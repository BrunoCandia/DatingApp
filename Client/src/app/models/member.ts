import { Photo } from "./photo";

export interface Member {
  userId: string;
  userName: string;
  age: number;
  knownAs: string;
  createdAt: Date;
  lastActive: Date;
  gender: string;
  introduction: string;
  interests: string;
  lookingFor: string;
  city: string;
  country: string;
  photoUrl: string;
  photos: Photo[];
}
