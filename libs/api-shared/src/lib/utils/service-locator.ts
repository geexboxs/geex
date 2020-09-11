import { ModuleRef } from "@nestjs/core";
import { INestApplication } from "@nestjs/core";

export class ServiceLocator {
    static config(app: INestApplication) {
        ServiceLocator._moduleRef = app;
    }
    private static _moduleRef: INestApplication;

    static get instance() {
        return ServiceLocator._moduleRef;
    }

    private constructor() {
    }
}
