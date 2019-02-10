import { Photo } from './photo';
import { PhotoSchedule } from './photo-schedule';

export interface User {
    id: number;
    username: string;
    department: string;
    type: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    photoUrlSched: string;
    city: string;
    country: string;
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    photos?: Photo[];
    photoSchedules?: PhotoSchedule[];
}
