import { Document } from "camo";

export class Movie extends Document<Movie> {
    title: StringConstructor;
    rating: { type: StringConstructor; choices: string[]; };
    releaseDate: DateConstructor;
    hasCreditCookie: BooleanConstructor;
    constructor() {
        super();

        this.title = String;
        this.rating = {
            type: String,
            choices: ['G', 'PG', 'PG-13', 'R']
        };
        this.releaseDate = Date;
        this.hasCreditCookie = Boolean;
    }

    static collectionName() {
        return 'movies';
    }
}
