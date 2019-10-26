import { Inject, Injectable } from "@graphql-modules/di";
import winston = require("winston");
import { LoggerConfigToken } from "../tokens";
import { LogTarget, LogLevel, ILoggerConfig } from "../../types";

@Injectable()
export class GeexLogger {
    public logger: winston.Logger;
    public target: LogTarget = "console";
    public filterLevel: LogLevel = "debug";

    /**
     *
     */
    constructor(@Inject(LoggerConfigToken) config?: ILoggerConfig) {
        if (config && config.target) {
            this.target = config.target;
        }
        if (config && config.filterLevel) {
            this.filterLevel = config.filterLevel;
        }

        switch (this.target) {
            default:
                {
                    const logger = winston.createLogger({
                        format: winston.format.combine(
                            winston.format.timestamp(),
                            winston.format.json(),
                        ),
                        // levels: ["debug", "error", "info", "warn"],
                        level: this.filterLevel,
                        defaultMeta: config ? config.metadata : undefined,
                        transports: [
                            new winston.transports.Console({
                                level: "debug", handleExceptions: true,
                                format: winston.format.combine(winston.format.cli({ level: true, all: true }), winston.format.colorize()),
                            }),
                        ],
                    });
                    this.logger = logger;
                }
                break;
        }
    }
    public error(error?: Error, ...args: any[]) {
        this.log("error", error, args);
    }
    public debug(...args: any[]) {
        this.log("debug", args);
    }
    public info(...args: any[]) {
        this.log("info", args);
    }
    public warn(...args: any[]);
    public warn(error?: Error, ...args: any[]) {
        this.log("warn", error, args);
    }
    public log(level: LogLevel, ...args: any[]) {
        // const NowDate = new Date().toISOString();
        // replace with more sophisticated solution :)
        switch (level) {
            case "debug":
                this.logger.debug(args);
                break;
            case "info":
                this.logger.info(args);
                break;
            case "warn":
                this.logger.warn(args);
                break;
            case "error":
                this.logger.error(args);
                break;
            default:
                break;
        }
    }
}
