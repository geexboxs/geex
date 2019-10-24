import bcrypt from "bcrypt";
export class Hasher {

    public hash(str: string): string {
        return bcrypt.hashSync(str, "");
    }
}
