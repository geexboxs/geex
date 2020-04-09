import OpentracingExtension, { RequestStart, ExtendedGraphQLResolveInfo } from "apollo-opentracing";
import { Inject, Injectable, ProviderScope } from "@graphql-modules/di";
import { GeexServerConfigToken } from "../tokens";
import { initTracer, TracingConfig, Reporter } from "jaeger-client";
import { GeexLogger } from "../utils/logger";
import { Span } from "opentracing";
import { ExecutionContext } from "@nestjs/common";
import { IGeexServerConfig } from "../../configs/types";
import { IGeexRequestStart, IGeexRequestEnd } from "../../types";

@Injectable({
    scope: ProviderScope.Request,
})
export class JaegerTraceExtension extends OpentracingExtension<any> {

    /**
     *
     */
    constructor(@Inject(GeexServerConfigToken) config: IGeexServerConfig, logger: GeexLogger) {
        const tracer = initTracer(config.traceConfig, {
            logger: {
                error: (msg) => logger.error(new Error(msg)),
                info: (msg) => logger.info(msg),
            },
            tags: {
                serverAddress: config.hostname,
            },
        });
        super({
            server: tracer,
            local: tracer,
            shouldTraceRequest: (info: Partial<IGeexRequestStart>) => {
                return true;
            },
        });
        // tslint:disable-next-line: no-string-literal
        const onRequestResolve = this["onRequestResolve"];
        // tslint:disable-next-line: no-string-literal
        this["onRequestResolve"] = (rootSpan: Span, infos: IGeexRequestStart) => {
            // tslint:disable-next-line: no-unnecessary-initializer
            let operation: string = "__unknown__";
            if (infos.requestContext.request.query!.startsWith("{")) {
                operation = "query";
            } else if (infos.requestContext.request.query!.startsWith("mutation")) {
                operation = "mutation";
            } else if (infos.requestContext.request.query!.startsWith("subscription")) {
                operation = "subscription";
            }
            rootSpan.setOperationName(operation);
            rootSpan.addTags({
                operation,
                ip: infos.context.req.connection.remoteAddress,
            });
            rootSpan.log({
                request: infos.requestContext.request,
            });
            // tslint:disable-next-line: no-string-literal
            infos.context.res.setHeader("X-B3-TraceId", rootSpan.context()["traceIdStr"]);
            // tslint:disable-next-line: no-string-literal
            this["willSendResponse"] = (res: IGeexRequestEnd) => {
                rootSpan.log({
                    headers: res.context.res.getHeaders(),
                });
                rootSpan.log({
                    response: res.graphqlResponse,
                });
            };
            return onRequestResolve(rootSpan, infos);
        };
    }

    willResolveField(source: any,
        args: { [argName: string]: any },
        // tslint:disable-next-line: align
        context: ExecutionContext,
        info: ExtendedGraphQLResolveInfo) {
        if (info.span) {
            info.span.log({ args });
        }
        return super.willResolveField(source, args, context, info);
    }
}
