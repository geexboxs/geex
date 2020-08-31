import { Inject, Injectable } from "@graphql-modules/di";
import winston = require("winston");
import { LogLevel } from "@nestjs/common";
import { GeexServerConfigToken } from "../tokens";
import { LogTarget } from "../../types";
import { IGeexServerConfig } from "../../configs/types";

@Injectable()
export class GeexLogger {
    public logger: winston.Logger;
    public target: LogTarget = "console";
    public filterLevel: LogLevel = "debug";

    /**
     *
     */
    constructor(@Inject(GeexServerConfigToken) serverConfig?: IGeexServerConfig) {
        let config = serverConfig?.loggerConfig;
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
        this.logger.error(error?.message ?? "error", error, args);
    }
    public debug(...args: any[]) {
        this.logger.debug(args);
    }
    public log(message: string, ...args: any[]) {
        this.logger.log("info", message, args);
    }
    public warn(...args: any[]);
    public warn(error?: Error, ...args: any[]) {
        this.logger.warn("warn", error, args);
    }
}
