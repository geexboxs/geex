import { Inject } from '@nestjs/common';
import { CommandHandler, ICommandHandler } from '@nestjs/cqrs';
import { InjectModel } from '@nestjs/mongoose';
import { ModelType } from '@typegoose/typegoose/lib/types';
import { SendUserRegisteredEmailCommand } from '../../account/commands/send-user-registered-email.command';
import { User } from '../../account/models/user.model';

@CommandHandler(SendUserRegisteredEmailCommand)
export class SendNotificationHandler implements ICommandHandler<SendUserRegisteredEmailCommand> {
    constructor(
        @Inject(User)
        private userModel: ModelType<User>,
    ) { }

    async execute(command: SendUserRegisteredEmailCommand) {
        throw Error("test")
    }
}
