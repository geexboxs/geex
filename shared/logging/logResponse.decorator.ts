import { UserResolver } from "../../app/user/user.resolver";
import { AppModule } from "../../app/app.module";

export const LogResponseList: (string | symbol)[] = [];
let resolverFields: any[] = [];
/**
 * Comment
 *
 * @returns {MethodDecorator}
 */
export function LogResponse(): MethodDecorator {
    return function (target: any, propertyKey: string | symbol, descriptor: PropertyDescriptor): PropertyDescriptor {
        setTimeout(() => {
            let query = AppModule.schema.getTypeMap()["Query"]
            let mutation = AppModule.schema.getTypeMap()["Mutation"]
            let subscription = AppModule.schema.getTypeMap()["Subscription"]
            if (!resolverFields.length) {
                resolverFields = Object.keys(query["_fields"]).concat(Object.keys(mutation["_fields"]))
                    /* .concat(Object.values(subscription["_fields"])) */;
            }
            if (!resolverFields.includes(propertyKey)) {
                return;
            }
            if (!LogResponseList.includes(propertyKey)) {
                LogResponseList.push(propertyKey)
            }
        }, 1000);
        return descriptor;
    }
}
