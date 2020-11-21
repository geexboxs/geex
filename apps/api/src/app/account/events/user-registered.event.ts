import { ObjectId } from 'bson';

export class UserRegisteredEvent {
  constructor(public readonly userId: ObjectId) {

  }
}
