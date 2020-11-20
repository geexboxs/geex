import * as jssha  from "jssha";

export class Hasher {

    public hash(str: string): string {
        let shaObj = new jssha("SHA-512", "TEXT");
        shaObj.update(str);
        return shaObj.getHash("HEX");
    }
}
