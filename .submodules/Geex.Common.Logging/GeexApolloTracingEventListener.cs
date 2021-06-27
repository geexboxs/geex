using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Geex.Common.Abstractions;
using Geex.Common.Logging;

using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Options;
using HotChocolate.Execution.Processing;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Geex.Common.Gql
{
    internal class GeexApolloTracingDiagnosticEventListener : DiagnosticEventListener
    {
        private const string _extensionKey = "tracing";
        private readonly ILogger<GeexApolloTracingDiagnosticEventListener> logger;
        private TracingPreference _tracingPreference;
        private readonly ITimestampProvider _timestampProvider;

        public GeexApolloTracingDiagnosticEventListener(
          ILogger<GeexApolloTracingDiagnosticEventListener> logger,
          LoggingModuleOptions options,
          ITimestampProvider? timestampProvider)
        {
            this.logger = logger;
            this._tracingPreference = options.TracingPreference;
            this._timestampProvider = timestampProvider;
        }

        public override bool EnableResolveFieldValue => true;

        public override IActivityScope ExecuteRequest(IRequestContext context)
        {
            if (!this.IsEnabled(context.ContextData))
                return this.EmptyScope;
            DateTime startTime = this._timestampProvider.UtcNow();
            logger.LogInformationWithData(new EventId((nameof(GeexApolloTracingRequestScope) + "Start").GetHashCode(), nameof(GeexApolloTracingRequestScope) + "Start"), "Request started.", new { QueryId = context.Request.QueryId, Query = context.Request.Query.ToString(), Variables = context.Request.VariableValues?.ToDictionary(x => x.Key, x => (x.Value as IValueNode)?.Value) });
            GeexApolloTracingResultBuilder builder = CreateBuilder(context.ContextData, logger);
            builder.SetRequestStartTime(startTime, this._timestampProvider.NowInNanoseconds());
            return new GeexApolloTracingRequestScope(context, logger, startTime, builder, this._timestampProvider);
        }

        public override IActivityScope ParseDocument(IRequestContext context)
        {
            GeexApolloTracingResultBuilder builder;
            return !TryGetBuilder(context.ContextData, out builder) ? this.EmptyScope : new ParseDocumentScope(builder, this._timestampProvider);
        }

        public override IActivityScope ValidateDocument(IRequestContext context)
        {
            GeexApolloTracingResultBuilder builder;
            return !TryGetBuilder(context.ContextData, out builder) ? this.EmptyScope : new ValidateDocumentScope(builder, this._timestampProvider);
        }

        public override IActivityScope ResolveFieldValue(IMiddlewareContext context)
        {
            GeexApolloTracingResultBuilder builder;
            return !TryGetBuilder(context.ContextData, out builder) ? this.EmptyScope : new ResolveFieldValueScope(context, builder, this._timestampProvider);
        }

        private static GeexApolloTracingResultBuilder CreateBuilder(IDictionary<string, object?> contextData,
            ILogger<GeexApolloTracingDiagnosticEventListener> logger)
        {
            GeexApolloTracingResultBuilder tracingResultBuilder = new GeexApolloTracingResultBuilder(logger);
            contextData["ApolloTracingResultBuilder"] = tracingResultBuilder;
            return tracingResultBuilder;
        }

        private static bool TryGetBuilder(
          IDictionary<string, object?> contextData,
          [NotNullWhen(true)] out GeexApolloTracingResultBuilder? builder)
        {
            object obj;
            if (contextData.TryGetValue("ApolloTracingResultBuilder", out obj) && obj is GeexApolloTracingResultBuilder tracingResultBuilder)
            {
                builder = tracingResultBuilder;
                return true;
            }
            builder = null;
            return false;
        }

        private bool IsEnabled(IDictionary<string, object?> contextData)
        {
            if (this._tracingPreference == TracingPreference.Always)
                return true;
            return this._tracingPreference == TracingPreference.OnDemand && contextData.ContainsKey("HotChocolate.Execution.EnableTracing");
        }

        private class GeexApolloTracingRequestScope : IActivityScope, IDisposable
        {
            private readonly IRequestContext _context;
            private readonly ILogger<GeexApolloTracingDiagnosticEventListener> _logger;
            private readonly DateTime _startTime;
            private readonly GeexApolloTracingResultBuilder _builder;
            private readonly ITimestampProvider _timestampProvider;
            private bool _disposed;

            public GeexApolloTracingRequestScope(IRequestContext context,
                ILogger<GeexApolloTracingDiagnosticEventListener> logger,
                DateTime startTime,
                GeexApolloTracingResultBuilder builder,
                ITimestampProvider timestampProvider)
            {
                this._context = context;
                this._logger = logger;
                this._startTime = startTime;
                this._builder = builder;
                this._timestampProvider = timestampProvider;
            }

            public void Dispose()
            {
                if (this._disposed)
                    return;
                this._builder.SetRequestDuration(this._timestampProvider.UtcNow() - this._startTime);

                if (this._context.Result is IReadOnlyQueryResult result)
                {
                    var resultMap = this._builder.Build();
                    this._logger.LogTraceWithData(GeexboxEventId.ApolloTracing, null, resultMap);
                    _logger.LogInformationWithData(new EventId((nameof(GeexApolloTracingRequestScope) + "End").GetHashCode(), nameof(GeexApolloTracingRequestScope) + "End"), "Request ended.", new { Label = result.Label, QueryId = this._context.Request.QueryId, Data = result.Data, Error = this._context.Result.Errors });
                    this._context.Result = QueryResultBuilder.FromResult(result).AddExtension("tracing", resultMap).Create();
                }

                this._disposed = true;
            }
        }

        private class ParseDocumentScope : IActivityScope, IDisposable
        {
            private readonly GeexApolloTracingResultBuilder _builder;
            private readonly ITimestampProvider _timestampProvider;
            private readonly long _startTimestamp;
            private bool _disposed;

            public ParseDocumentScope(
              GeexApolloTracingResultBuilder builder,
              ITimestampProvider timestampProvider)
            {
                this._builder = builder;
                this._timestampProvider = timestampProvider;
                this._startTimestamp = timestampProvider.NowInNanoseconds();
            }

            public void Dispose()
            {
                if (this._disposed)
                    return;
                this._builder.SetParsingResult(this._startTimestamp, this._timestampProvider.NowInNanoseconds());
                this._disposed = true;
            }
        }

        private class ValidateDocumentScope : IActivityScope, IDisposable
        {
            private readonly GeexApolloTracingResultBuilder _builder;
            private readonly ITimestampProvider _timestampProvider;
            private readonly long _startTimestamp;
            private bool _disposed;

            public ValidateDocumentScope(
              GeexApolloTracingResultBuilder builder,
              ITimestampProvider timestampProvider)
            {
                this._builder = builder;
                this._timestampProvider = timestampProvider;
                this._startTimestamp = timestampProvider.NowInNanoseconds();
            }

            public void Dispose()
            {
                if (this._disposed)
                    return;
                this._builder.SetValidationResult(this._startTimestamp, this._timestampProvider.NowInNanoseconds());
                this._disposed = true;
            }
        }

        private class ResolveFieldValueScope : IActivityScope, IDisposable
        {
            private readonly IMiddlewareContext _context;
            private readonly GeexApolloTracingResultBuilder _builder;
            private readonly ITimestampProvider _timestampProvider;
            private readonly long _startTimestamp;
            private bool _disposed;

            public ResolveFieldValueScope(
              IMiddlewareContext context,
              GeexApolloTracingResultBuilder builder,
              ITimestampProvider timestampProvider)
            {
                this._context = context;
                this._builder = builder;
                this._timestampProvider = timestampProvider;
                this._startTimestamp = timestampProvider.NowInNanoseconds();
            }

            public void Dispose()
            {
                if (this._disposed)
                    return;
                this._builder.AddResolverResult(new GeexApolloTracingResolverRecord(this._context, this._startTimestamp, this._timestampProvider.NowInNanoseconds()));
                this._disposed = true;
            }
        }
    }

    internal class GeexApolloTracingResultBuilder
    {
        private readonly ILogger<GeexApolloTracingDiagnosticEventListener> logger;
        private const int _apolloTracingVersion = 1;
        private const long _ticksToNanosecondsMultiplicator = 100;
        private readonly ConcurrentQueue<GeexApolloTracingResolverRecord> _resolverRecords = new ConcurrentQueue<GeexApolloTracingResolverRecord>();
        private TimeSpan _duration;
        private ResultMap? _parsingResult;
        private DateTimeOffset _startTime;
        private long _startTimestamp;
        private ResultMap? _validationResult;

        public GeexApolloTracingResultBuilder(ILogger<GeexApolloTracingDiagnosticEventListener> logger)
        {
            this.logger = logger;
        }

        public void SetRequestStartTime(DateTimeOffset startTime, long startTimestamp)
        {
            this._startTime = startTime;
            this._startTimestamp = startTimestamp;
            logger.LogTrace(GeexboxEventId.ApolloTracing, "request started.");
        }

        public void SetParsingResult(long startTimestamp, long endTimestamp)
        {
            this._parsingResult = new ResultMap();
            this._parsingResult.EnsureCapacity(2);
            this._parsingResult.SetValue(0, "startOffset", startTimestamp - this._startTimestamp);
            this._parsingResult.SetValue(1, "duration", endTimestamp - startTimestamp);
        }

        public void SetValidationResult(long startTimestamp, long endTimestamp)
        {
            this._validationResult = new ResultMap();
            this._validationResult.EnsureCapacity(2);
            this._validationResult.SetValue(0, "startOffset", startTimestamp - this._startTimestamp);
            this._validationResult.SetValue(1, "duration", endTimestamp - startTimestamp);
        }

        public void AddResolverResult(GeexApolloTracingResolverRecord record) => this._resolverRecords.Enqueue(record);

        public void SetRequestDuration(TimeSpan duration) => this._duration = duration;

        public IResultMap Build()
        {
            if (this._parsingResult == null)
                this.SetParsingResult(this._startTimestamp, this._startTimestamp);
            if (this._validationResult == null)
                this.SetValidationResult(this._startTimestamp, this._startTimestamp);
            ResultMap resultMap1 = new ResultMap();
            resultMap1.EnsureCapacity(1);
            resultMap1.SetValue(0, "resolvers", this.BuildResolverResults());
            ResultMap resultMap2 = new ResultMap();
            resultMap2.EnsureCapacity(7);
            resultMap2.SetValue(0, "version", 1);
            resultMap2.SetValue(1, "startTime", this._startTime.ToUnixTimeSeconds());
            resultMap2.SetValue(2, "endTime", this._startTime.Add(this._duration).ToUnixTimeSeconds());
            resultMap2.SetValue(3, "duration", this._duration.TotalSeconds);
            resultMap2.SetValue(4, "parsing", (object)this._parsingResult);
            resultMap2.SetValue(5, "validation", (object)this._validationResult);
            resultMap2.SetValue(6, "execution", resultMap1);
            return resultMap2;
        }

        private ResultMap[] BuildResolverResults()
        {
            int num = 0;
            ResultMap[] resultMapArray = new ResultMap[this._resolverRecords.Count];
            foreach (GeexApolloTracingResolverRecord resolverRecord in this._resolverRecords)
            {
                ResultMap resultMap = new ResultMap();
                resultMap.EnsureCapacity(6);
                resultMap.SetValue(0, "path", resolverRecord.Path);
                resultMap.SetValue(1, "parentType", resolverRecord.ParentType);
                resultMap.SetValue(2, "fieldName", resolverRecord.FieldName);
                resultMap.SetValue(3, "returnType", resolverRecord.ReturnType);
                resultMap.SetValue(4, "startOffset", resolverRecord.StartTimestamp - this._startTimestamp);
                resultMap.SetValue(5, "duration", resolverRecord.EndTimestamp - resolverRecord.StartTimestamp);
                resultMapArray[num++] = resultMap;
            }
            return resultMapArray;
        }
    }

    internal class GeexApolloTracingResolverRecord
    {
        public GeexApolloTracingResolverRecord(
          IResolverContext context,
          long startTimestamp,
          long endTimestamp)
        {
            this.Path = context.Path.ToList();
            this.ParentType = context.ObjectType.Name;
            this.FieldName = context.Field.Name;
            this.ReturnType = context.Field.Type.TypeName();
            this.StartTimestamp = startTimestamp;
            this.EndTimestamp = endTimestamp;
        }

        public IReadOnlyList<object> Path { get; }

        public string ParentType { get; }

        public string FieldName { get; }

        public string ReturnType { get; }

        public long StartTimestamp { get; }

        public long EndTimestamp { get; }
    }
}
