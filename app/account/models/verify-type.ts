import { registerEnumType } from "@nestjs/graphql";

export enum VerifyType {
    Email = "Email",
    Sms = "Sms",
}

registerEnumType(VerifyType, {
    name: "VerifyType", // this one is mandatory
    description: "verify types, email or sms", // this one is optional
});
