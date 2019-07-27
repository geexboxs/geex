import { AppModule } from "../../app/app.module";
import { GeexLogger } from "./logger";
import { LogResponseList } from "./logResponse.decorator";
import { isArray, isString } from "util";

const globalLogger = new GeexLogger();
export const globalLoggingMiddleware = function (req, res, next) {
    const url = req.headers.referer
    const query = req.body.query
    if (req.body && req.body.query) {
        globalLogger.debug(url, query);
    }
    if (/{[\s|.|\n]*(\w+)\s{/.exec(query) === null) {
        next();
        return;
    }
    var resolverFieldName = /{[\s|.|\n]*(\w+)\s{/.exec(query)![1];

    let oldWrite = res.write,
        oldEnd = res.end;

    let chunks: any[] = [];

    res.write = function (chunk) {
        chunks.push(chunk);

        oldWrite.apply(res, arguments);
    };

    res.end = function (chunk) {
        if (chunk)
            chunks.push(chunk);
        let firstChunk = chunks[0]
        if (isString(firstChunk)) {
            if (firstChunk.startsWith("{\"error")) {
                globalLogger.error(new Error(firstChunk))
                
            } else {
                globalLogger.info(firstChunk)
            }

        } else if (LogResponseList.includes(resolverFieldName)) {
            let body = Buffer.concat(chunks).toString('utf8');
        }
        oldEnd.apply(res, arguments);

    }
    next();
};
