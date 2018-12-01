import { Photo } from './photo';

export interface User {
    // replicate what is returning for our user.
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    // ? for optional. elvis operator
    interests?: string;
    introduction?: string;
    lookingfor?: string;
    photos?: Photo[];
}
