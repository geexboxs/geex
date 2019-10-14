import bcrypt from "bcrypt";
export class PasswordHasher {
    /**
     *
     */
    constructor(private _salt: string) {

    }
    public hash(password: string): string {
        return bcrypt.hashSync(password, this._salt);
    }
}
