using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTests.Extensions;

namespace UnitTests.Mock
{
    public class LoggerMock : ILogger
    {
        public List<LoggedMessage> Messages { get; set; }

        public Mock<ILogger> Mock { get; private set; }

        public ILogger Object => this.Mock.Object;

        public LoggerMock() : this(LogLevel.Information)
        { }

        public LoggerMock(LogLevel level = LogLevel.Information)
        {
            Messages = new List<LoggedMessage>();

            this.Mock = new Mock<ILogger>();
            this.Mock
                .Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback((IInvocation invocation) =>
                {
                    Messages.Add(new LoggedMessage
                    {
                        Level = (LogLevel)invocation.Arguments[0],
                        Exception = (Exception)invocation.Arguments[3],
                        Message = invocation.Arguments[2].ToString()
                    });
                });

            this.Mock.Setup(x => x.IsEnabled(It.IsAny<LogLevel>()))
                .Returns<LogLevel>(lvl => lvl == level);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this.Object.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this.Object.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.Object.Log<TState>(logLevel, eventId, state, exception, formatter);
        }

        public void VerifyLog(LogLevel logLevel, Func<string?, bool> messageMatch, int? times = null, Func<Exception, bool>? exceptionMatch = null, string? failMessage = null)
        {
            var count = this.Messages
                .Where(x => x.Level == logLevel)
                .AsQueryable()
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                .WhereIf(exceptionMatch != null, x => x.Exception != null && exceptionMatch(x.Exception))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .Select(x => x.Message)
                .Count(messageMatch);

            if (times.HasValue)
                Assert.AreEqual(times, count, failMessage);
            else
                Assert.IsTrue(count > 0, failMessage);
        }

        public class LoggedMessage
        {
            public string? Message { get; set; }

            public LogLevel Level { get; set; }

            public Exception? Exception { get; set; }
        }
    }

    public class LoggerMock<T> : LoggerMock, ILogger<T>
    {
    }
}
