import winston from "winston";
import { Injectable, Inject } from "@graphql-modules/di";
import { LoggerConfigToken } from "../logging/tokens";
type LogLevel = "debug" | "info" | "warn" | "error";

type LogTarget = "console" | "file" | "remote";

export interface LoggerConfig {
    /**
     *log target
     *
     * @type {LogTarget}
     * @default "console"
     * @memberof LoggerConfig
     */
    target?: LogTarget;
    /**
     *minimal level to log
     *
     * @type {LogLevel}
     * @default "info"
     * @memberof LoggerConfig
     */
    filterLevel?: LogLevel;
    consoleConfig?: {}
    fileConfig?: {}
    remoteConfig?: {}
    /**
     *metadata to be logged in every log entry
     *
     * @type {{}}
     * @memberof LoggerConfig
     */
    metadata?: {}
}

@Injectable()
export class GeexLogger {
    _logger: winston.Logger;
    error(error?: Error, ...args: any[]) {
        this.log("error", error, args)
    }
    _target: LogTarget = "console";
    _filterLevel: LogLevel = "debug";
    debug(...args: any[]) {
        this.log("debug", args)
    }
    info(...args: any[]) {
        this.log("info", args)
    }
    warn(...args: any[])
    warn(error?: Error, ...args: any[]) {
        this.log("warn", error, args)
    }

    /**
     *
     */
    constructor(@Inject(LoggerConfigToken) config?: LoggerConfig) {
        if (config && config.target) {
            this._target = config.target;
        }
        if (config && config.filterLevel) {
            this._filterLevel = config.filterLevel;
        }

        switch (this._target) {
            default:
                {
                    const logger = winston.createLogger({
                        format: winston.format.combine(winston.format.timestamp(), winston.format.json()),
                        // levels: ["debug", "error", "info", "warn"],
                        level: this._filterLevel,
                        defaultMeta: config ? config.metadata : undefined,
                        transports: [
                            new winston.transports.Console({ level: "debug", handleExceptions: true, format: winston.format.combine(winston.format.timestamp(), winston.format.cli({ level: true })) }),
                        ]
                    });
                    this._logger = logger;
                }
                break;
        }
    }
    log(level: LogLevel, ...args: any[]) {
        // const NowDate = new Date().toISOString();
        // replace with more sophisticated solution :)
        switch (level) {
            case "debug":
                this._logger.debug(args);
                break;
            case "info":
                this._logger.info(args);
                break;
            case "warn":
                this._logger.warn(args);
                break;
            case "error":
                this._logger.error(args);
                break;
            default:
                break;
        }
    }
}
