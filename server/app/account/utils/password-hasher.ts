import { Hasher } from "../../../shared/utils/hasher";
import jssha = require("jssha");
export class PasswordHasher extends Hasher {

    /**
     *
     */
    constructor(private secrect: string) {
        super();
    }
    public hash(str: string): string {
        let shaObj = new jssha("SHA-512", "TEXT");
        shaObj.setHMACKey(this.secrect, "TEXT");
        shaObj.update(str);
        return shaObj.getHMAC("HEX");
    }
    verify(password: string, passwordHash: string) {
        let shaObj = new jssha("SHA-512", "TEXT");
        shaObj.setHMACKey(this.secrect, "TEXT");
        shaObj.update(password);
        return shaObj.getHMAC("HEX") == passwordHash;
    }
}
