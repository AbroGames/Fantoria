using Fantoria.Scenes.Game.Starters;
using Godot;

namespace Fantoria.Scenes.Game;

public partial class Game : Node2D
{
    
    public Network Network { get; private set; } //TODO readonly?

    public Game Init(BaseGameStarter gameStarter)
    {
        gameStarter.Init(this);
        return this;
    }
}
        
//TODO какая структура папок Screen, учитывая что половина игры -- это меню? Вторая половина -- бои. Внутри Game снова разделение на два типа?
//TODO учитывая сеть нужна возможность "сворачивать" бой и возвращаться к обычной игре, и боев может быть несколько одновременно. И действовать там могут все игроки (по дефолту).
        
//TODO Потребуются GameState и машина состояний? Как в Neon, но полноценная? Или разделить типа на бек и фронт, и все валидировать на "беке"?
//TODO Возможность загрузки игры из файла сейва из какого-то состояния? По сути это один-в-один логика как синк с сервером.
//TODO PingService / PingNode стырить из Neon сюда в Lib, без сетевой логики, чисто как аналитика и record пакета
        
//TODO В каждой ноде один раз получать в _Ready текущий Game через сервис? Или через родителей? А все остальные GameServices вложены в Game?

//TODO Чекнуть все ошибки it can be readonly (создать такую искусственно, а потом поискать по всем)