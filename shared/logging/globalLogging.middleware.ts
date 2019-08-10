import { AppModule } from "../../app/app.module";
import { GeexLogger } from "./logger";
import { LogResponseList } from "./logResponse.decorator";
import { isArray, isString } from "util";
import { Request, Response } from "express";
import { OutgoingHttpHeaders, IncomingHttpHeaders } from "http";
import stringify from 'json-stringify-safe';

const globalLogger = new GeexLogger();
export const globalLoggingMiddleware = function (req: Request, res: Response, next) {
    const query = req.body.query
    let logContent: { req: { headers: IncomingHttpHeaders, body: string }, res?: { headers: OutgoingHttpHeaders, body: string } };
    if (req.body && req.body.query && !req.body.query.startsWith("query IntrospectionQuery")) {
        logContent = { req: { headers: req.headers, body: req.body } }
    }
    if (/{[\s|.|\n]*(\w+)\s{/.exec(query) === null) {
        return next();
    }
    var resolverFieldName = /{[\s|.|\n]*(\w+)\s{/.exec(query)![1];

    let oldWrite = res.write,
        oldEnd = res.end;

    let chunks: any[] = [];
    res.write = function (chunk) {
        chunks.push(chunk);

        return oldWrite.apply(res, arguments as any);
    };
    res.end = function () {
        let responseContent = chunks.join();
        if (LogResponseList.includes(resolverFieldName)) {
            logContent.res = { headers: res.getHeaders(), body: responseContent };
        }
        if (logContent) {
            globalLogger.debug(stringify(logContent));
        }
        oldEnd.apply(res, arguments as any);

    }
    return next();
};
