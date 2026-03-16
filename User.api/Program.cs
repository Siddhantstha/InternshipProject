using System.Text;
using Application.Interface;
using Application.Service;
using Application.Validator;
using Confluent.Kafka;
using FluentValidation;
using Infrastructure.DBconnect;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDBconnect>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=user;Username=postgres;Password=koeJ2449k");
});
builder.Services.ApiDI();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserDetails, UserDetailsService>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddScoped<ValidationActionFilterAttribute>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)
        )
    };
});
 static IConfiguration readConfig()
{
	// reads the client configuration from client.properties
	// and returns it as a configuration object
	return new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddIniFile("client.properties", false)
	.Build();
}

static void produce(string topic, IConfiguration config)
{
	// creates a new producer instance
	using (var producer = new ProducerBuilder<string, string>(config.AsEnumerable()).Build())
	{
		// produces a sample message to the user-created topic and prints
		// a message when successful or an error occurs
		producer.Produce(topic, new Message<string, string> { Key = "key", Value = "value" },
		  (deliveryReport) => {
			  if (deliveryReport.Error.Code != ErrorCode.NoError)
			  {
				  Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
			  }
			  else
			  {
				  Console.WriteLine($"Produced event to topic {topic}: key = {deliveryReport.Message.Key,-10} value = {deliveryReport.Message.Value}");
			  }
		  }
		);

		// send any outstanding or buffered messages to the Kafka broker
		producer.Flush(TimeSpan.FromSeconds(10));
	}
}

static void consume(string topic, IConfiguration config)
{
	config["group.id"] = "csharp-group-1";
	config["auto.offset.reset"] = "earliest";

	// creates a new consumer instance
	using (var consumer = new ConsumerBuilder<string, string>(config.AsEnumerable()).Build())
	{
		consumer.Subscribe(topic);
		while (true)
		{
			// consumes messages from the subscribed topic and prints them to the console
			var cr = consumer.Consume();
			Console.WriteLine($"Consumed event from topic {topic}: key = {cr.Message.Key,-10} value = {cr.Message.Value}");
		}

		// closes the consumer connection
		consumer.Close();
	}
}

static void Main(string[] args)
{
	// producer and consumer code here
	IConfiguration config = readConfig();
	const string topic = "topic_1";

	produce(topic, config);
	consume(topic, config);
}
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Swagger UI available at /swagger
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Swagger UI available at /swagger
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
