export interface PhotoSchedule {
    id: number;
    url: string;
    description: string;
    dateAdded: Date;
    isMain?: boolean;
    isMainSched?: boolean;
}
