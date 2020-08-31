import { Injectable, NestInterceptor, ExecutionContext, CallHandler, INestApplicationContext, UseInterceptors } from "@nestjs/common";
import { Observable, of } from "rxjs";
import { AuditLog } from "../../app/audit-log/audit-log.model";

@Injectable()
export class AuditInterceptor implements NestInterceptor {
    /**
     *
     */
    constructor() {

    }
    async intercept(context: ExecutionContext, next: CallHandler): Promise<Observable<any>> {
        let auditLog = new AuditLog(context.toString());
        return next.handle();
    }
}


export const Audit = UseInterceptors.bind(AuditInterceptor) as () => MethodDecorator;
