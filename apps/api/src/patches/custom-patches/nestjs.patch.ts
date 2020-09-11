import { Type } from "@nestjs/common";
import { NestApplication } from "@nestjs/core";

NestApplication.prototype["getModel"] = function <T>(this: NestApplication, ctorOrName: Type<T> | string) {
    let modelName;
    if (typeof ctorOrName == "string") {
        modelName = ctorOrName;
    }
    else {
        modelName = ctorOrName.name;
    }
    return this.get(modelName + "Model");
};
