
import { Component, OnInit } from '@angular/core';
import { Card } from '../../components/card/card';
import { RawgService } from '../../services/rawg.service';
import { RawgGame } from '../../models/rawg-game.model';

@Component({
  selector: 'app-home',
  imports: [Card],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  games: RawgGame[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private rawgService: RawgService) {}

  ngOnInit(): void {
    this.rawgService.getGames().subscribe({
      next: (response) => {
        this.games = response.results;
        this.isLoading = false;
        console.log('Games fetched successfully:', this.games);
      },
      error: (error) => {
        console.error('Erro ao buscar jogos:', error);
        this.errorMessage = 'Não foi possível carregar os jogos. Tente novamente mais tarde.';
        this.isLoading = false;
      },
    });
  }
}
