import { ObjectId } from 'bson';


export class SendUserRegisteredEmailCommand {
    constructor(
        public readonly userId: ObjectId,
    ) { }
}
