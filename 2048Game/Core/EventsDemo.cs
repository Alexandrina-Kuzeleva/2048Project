using System;
using _2048Game.Systems;
using _2048Game.UI;

namespace _2048Game.Core
{
    public static class EventsDemo
    {
        public static void DemonstrateEvents()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 EVENTS PATTERN DEMONSTRATION              ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            var scoreManager = new ScoreManager();

            Console.WriteLine("▶ DEMO: Score events with HUD subscriber\n");
            Console.WriteLine("   Creating HUD (subscribes to ScoreChanged event)...");

            using (var hud = new ConsoleHUD(scoreManager))
            {
                Console.WriteLine("   HUD subscribed! Now adding points:\n");

                Console.WriteLine("   Adding 100 points...");
                scoreManager.AddPoints(100);

                System.Threading.Thread.Sleep(500);

                Console.WriteLine("\n   Adding 50 points...");
                scoreManager.AddPoints(50);

                System.Threading.Thread.Sleep(500);

                Console.WriteLine("\n   Adding 200 points (NEW HIGH SCORE!)...");
                scoreManager.AddPoints(200);

                System.Threading.Thread.Sleep(500);

                Console.WriteLine("\n   Resetting score...");
                scoreManager.ResetScore();
            }

            Console.WriteLine("\n✓ Events demonstrate loose coupling!");
            Console.WriteLine("  - ScoreManager doesn't know about HUD");
            Console.WriteLine("  - HUD automatically reacts to score changes");
            Console.WriteLine("  - Easy to add more subscribers\n");
        }
    }
}