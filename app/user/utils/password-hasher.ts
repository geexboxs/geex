import { Hasher } from "../../../shared/utils/hasher";
import * as jssha from "jssha";
export class PasswordHasher extends Hasher {
    /**
     *
     */
    constructor(private secrect: string) {
        super();
    }
    public hash(str: string): string {
        let shaObj = new jssha.default("SHA-512", "TEXT");
        shaObj.setHMACKey(this.secrect, "TEXT");
        shaObj.update(str);
        return shaObj.getHMAC("HEX");
    }
}
