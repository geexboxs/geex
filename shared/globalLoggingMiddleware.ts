import { AppModule } from "../app/app.module";
import { GeexLogger } from "./logging/Logger";


export const globalLoggingMiddleware = function (req, res, next) {
    if (req.body && req.body.query) {
        const url = req.headers.referer
        const query = req.body.query
        AppModule.injector.get(GeexLogger).debug(url, query);
    }
    next();
};
