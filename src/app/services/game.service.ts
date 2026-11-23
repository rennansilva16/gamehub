import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";

import { environment } from "../../environments/environment";
import { Game } from "../models/game.model";
import { GameApiResponse } from "../models/game-api-response.model";

@Injectable({
  providedIn: 'root'
})

export class GameService {
  private readonly apiUrl = `${environment.gameHubApiUrl}/games`;

  constructor(private http: HttpClient) { }

  getGames(): Observable<Game[]> {
    return this.http.get<GameApiResponse[]>(this.apiUrl).pipe(
      map((response) =>
        response.map((game) => ({
          id: game.id,
          name: game.name,
          cover: game.backgroundImage,
          platforms: [], // Assuming platforms are not provided in the API response
        })),
      ),
    )
  }
}
