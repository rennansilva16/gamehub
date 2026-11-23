import { Component, OnInit, signal } from '@angular/core';
import { Card } from '../../components/card/card';
import { GameService } from '../../services/game.service';
import { Game } from '../../models/game.model';

@Component({
  selector: 'app-home',
  imports: [Card],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {

  games = signal<Game[]>([]);
  isLoading = signal(true);
  errorMessage = signal('');

  constructor(private gameService: GameService) {}

  ngOnInit(): void {
    this.gameService.getGames().subscribe({
      next: (games) => {
        this.games.set(games);
        this.isLoading.set(false);

        console.log('Games fetched successfully:', this.games());
      },
      error: (error) => {
        console.error('Erro ao buscar jogos:', error);

        this.errorMessage.set(
          'Não foi possível carregar os jogos. Tente novamente mais tarde.'
        );

        this.isLoading.set(false);
      },
    });
  }
}
