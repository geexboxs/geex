import bcrypt from "bcrypt";
import { Hasher } from "../../../shared/utils/hasher";
export class PasswordHasher extends Hasher {
    /**
     *
     */
    constructor(private _salt: string) {
        super();
    }
    public hash(str: string): string {
        return bcrypt.hashSync(str, this._salt);
    }
}
