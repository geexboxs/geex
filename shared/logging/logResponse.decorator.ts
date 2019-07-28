export const LogResponseList: (string | symbol)[] = [];
/**
 * Comment
 *
 * @returns {MethodDecorator}
 */
export function LogResponse(): MethodDecorator {
    return function (target: any, propertyKey: string | symbol, descriptor: PropertyDescriptor): PropertyDescriptor {
        if (!LogResponseList.includes(propertyKey)) {
            LogResponseList.push(propertyKey)
        }
        return descriptor;
    }
}
