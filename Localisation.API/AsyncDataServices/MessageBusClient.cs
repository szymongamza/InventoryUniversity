﻿using Localisation.API.Dtos;
using AutoMapper;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace Localisation.API.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() {HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                UserName = _configuration["RabbitMQUser"],
                Password = _configuration["RabbitMQPass"]};
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void PublishNewRoom(RoomPublishedDto roomPublishedDto)
        {
            var message = JsonSerializer.Serialize(roomPublishedDto);

            if (_connection.IsOpen)
                SendMessage(message);
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
        }
    }
}