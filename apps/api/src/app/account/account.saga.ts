// import { Injectable } from '@nestjs/common';
// import { Saga, ICommand, ofType } from '@nestjs/cqrs';
// import { Observable } from 'rxjs';
// import { map } from 'rxjs/operators';
// import { SendUserRegisteredEmailCommand } from './commands/send-user-registered-email.command';
// import { UserRegisteredEvent } from './events/user-registered.event';

// @Injectable()
// export class AccountSagas {
//   @Saga()
//   dragonKilled = (events$: Observable<any>): Observable<ICommand> => {
//     return events$.pipe(
//       ofType(UserRegisteredEvent),
//       map((event) => new SendUserRegisteredEmailCommand(event.userId)),
//     );
//   }
// }
