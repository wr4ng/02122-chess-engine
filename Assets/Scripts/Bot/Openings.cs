using System.Collections.Generic;
namespace Bot
{
    public class Openings
    {
        private List<List<string>> openings = new List<List<string>>();
        private int openingIndex = 0;

        public Openings()
        {
            openings = new List<List<string>>
            {
                new List<string> {"e4", "e5", "d4", "exd4", "c3"},
                new List<string> {"e4"},
                new List<string> {"d4"},
                new List<string> {"g3"},
                new List<string> {"b3"},
                new List<string> {"c4"},
                new List<string> {"f4"},
                new List<string> {"Nf3"},
                new List<string> {"c4", "c5"},
                new List<string> {"d4", "d5"},
                new List<string> {"c4", "e5"},
                new List<string> {"d4", "g6"},
                new List<string> {"e4", "d6"},
                new List<string> {"d4", "c5"},
                new List<string> {"e4", "c6"},
                new List<string> {"e4", "e6"},
                new List<string> {"e4", "Nf6"},
                new List<string> {"e4", "d5"},
                new List<string> {"e4", "c5"},
                new List<string> {"e4", "c5", "c3"},
                new List<string> {"e4", "c5", "d4"},
                new List<string> {"e4", "e5", "Bc4"},
                new List<string> {"e4", "c5", "Nc3"},
                new List<string> {"d4", "d5", "e4"},
                new List<string> {"d4", "Nf6", "Bg5"},
                new List<string> {"Nf3", "d5", "c4"},
                new List<string> {"e4", "c5", "d4"},
                new List<string> {"e4", "c5", "Nc3"},
                new List<string> {"Nf3", "d5", "c4"},
                new List<string> {"d4", "Nf6", "Bg5"},
                new List<string> {"e4", "e5", "f4", "d5"},
                new List<string> {"d4", "d5", "c4", "Nc6"},
                new List<string> {"e4", "e5", "Nf3", "d6"},
                new List<string> {"d4", "Nf6", "c4", "d6"},
                new List<string> {"d4", "d5", "c4", "e5"},
                new List<string> {"e4", "d5", "exd5", "Nf6"},
                new List<string> {"d4", "Nf6", "c4", "e5"},
                new List<string> {"d4", "f5", "e4", "fxe4"},
                new List<string> {"e4", "e5", "f4", "Bc5"},
                new List<string> {"e4", "e5", "Nf3", "Nf6"},
                new List<string> {"d4", "Nf6", "c4", "g6"},
                new List<string> {"d4", "d5", "c4", "c6"},
                new List<string> {"d4", "d5", "c4", "dxc4"},
                new List<string> {"e4", "e5", "f4", "exf4"},
                new List<string> {"d4", "d5", "Nf3", "Nf6", "Bf4"},
                new List<string> {"d4", "d5", "Nf3", "Nf6", "Bg5"},
                new List<string> {"e4", "e6", "d4", "d5", "e5"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bb5"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "d4"},
                new List<string> {"e4", "e6", "d4", "d5", "Nd2"},
                new List<string> {"e4", "c6", "d4", "d5", "e5"},
                new List<string> {"e4", "c5", "Nc3", "Nc6", "f4"},
                new List<string> {"e4", "e5", "f4", "exf4", "Bc4"},
                new List<string> {"e4", "c6", "d4", "d5", "exd5"},
                new List<string> {"e4", "e5", "Nf3", "Nf6", "Nc3"},
                new List<string> {"e4", "e6", "d4", "d5", "exd5"},
                new List<string> {"e4", "e5", "Nf3", "Nf6", "d4"},
                new List<string> {"d4", "d5", "Nf3", "Nf6", "Bg5"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Nc3", "Nf6"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bc4", "Nf6"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bb5", "Nf6"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bc4", "Bc5"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "Bg7"},
                new List<string> {"e4", "e6", "d4", "d5", "Nc3", "Bb4"},
                new List<string> {"d4", "Nf6", "c4", "e6", "Nc3", "Bb4"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "d5"},
                new List<string> {"d4", "Nf6", "c4", "c5", "d5", "e6"},
                new List<string> {"d4", "Nf6", "c4", "c5", "d5", "b5"},
                new List<string> {"d4", "Nf6", "c4", "e6", "Nf3", "b6"},
                new List<string> {"e4", "e6", "d4", "d5", "Nc3", "Nf6"},
                new List<string> {"e4", "e5", "f4", "exf4", "Nf3", "d5"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bb5", "f5"},
                new List<string> {"e4", "g6", "d4", "Bg7", "Nc3", "c5"},
                new List<string> {"e4", "e6", "d4", "d5", "Nc3", "dxe4"},
                new List<string> {"d4", "Nf6", "c4", "e6", "Nf3", "Bb4+"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bb5", "d6"},
                new List<string> {"d4", "d5", "c4", "e6", "Nf3", "c5"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bb5", "Bc5"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bc4", "Be7"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bc4", "Bc5", "b4"},
                new List<string> {"e4", "c6", "d4", "d5", "exd5", "cxd5", "c4"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Nc3", "Nf6", "d4"},
                new List<string> {"d4", "Nf6", "c4", "e6", "g3", "d5", "Bg2"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "Bb5", "a6", "Bxc6"},
                new List<string> {"e4", "d6", "d4", "Nf6", "Nc3", "g6", "Nf3"},
                new List<string> {"e4", "d6", "d4", "Nf6", "Nc3", "g6", "f4"},
                new List<string> {"d4", "Nf6", "c4", "e6", "Nc3", "Bb4", "Qc2"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "d5", "Bg5"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "d5", "Nf3"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "d5", "Bf4"},
                new List<string> {"e4", "c5", "Nf3", "Nc6", "d4", "cxd4", "Nxd4"},
                new List<string> {"e4", "c6", "d4", "d5", "Nc3", "dxe4", "Nxe4", "Bf5"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "d4", "exd4", "Nxd4", "Bc5"},
                new List<string> {"d4", "d5", "c4", "e6", "Nc3", "Nf6", "Nf3", "c5"},
                new List<string> {"d4", "d5", "c4", "e6", "Nc3", "Nf6", "Nf3", "c6"},
                new List<string> {"e4", "c5", "Nf3", "e6", "d4", "cxd4", "Nxd4", "Nc6"},
                new List<string> {"e4", "e6", "d4", "d5", "Nc3", "Nf6", "Bg5", "dxe4"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "d4", "exd4", "Nxd4", "Qh4"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "d5", "cxd5", "Nxd5"},
                new List<string> {"d4", "Nf6", "c4", "e6", "Nc3", "Bb4", "e3", "c5"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "Bg7", "Nf3", "d6", "g3"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "Bg7", "e4", "d6", "f4"},
                new List<string> {"e4", "e5", "Nf3", "Nf6", "Nxe5", "d6", "Nf3", "Nxe4", "d4"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "Bg7", "e4", "d6", "f3"},
                new List<string> {"c4", "f5", "Nf3", "Nf6", "d4", "g6", "g3", "Bg7", "Bg2"},
                new List<string> {"d4", "Nf6", "c4", "e6", "Nc3", "Bb4", "a3", "Bxc3+", "bxc3"},
                new List<string> {"d4", "Nf6", "c4", "e6", "Nc3", "Bb4", "e3", "c5", "Ne2"},
                new List<string> {"e4", "c5", "Nf3", "d6", "d4", "cxd4", "Nxd4", "Nf6", "Nc3", "g6"},
                new List<string> {"e4", "c5", "Nf3", "d6", "d4", "cxd4", "Nxd4", "Nf6", "Nc3", "a6"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "Bg7", "e4", "d6", "Nf3", "O-O", "Be2"},
                new List<string> {"e4", "c5", "Nf3", "d6", "d4", "cxd4", "Nxd4", "Nf6", "Nc3", "Nc6", "Bg5"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "Bg7", "e4", "d6", "Be2", "O-O", "Bg5"},
                new List<string> {"e4", "e5", "Nf3", "Nc6", "d4", "exd4", "Nxd4", "Nf6", "Nxc6", "bxc6", "e5"},
                new List<string> {"d4", "d5", "c4", "e6", "Nc3", "Nf6", "Bg5", "Be7", "e3", "O-O Nf3", "Nbd7"},
                new List<string> {"d4", "f5", "c4", "Nf6", "g3", "e6", "Bg2", "Be7", "Nf3", "O-O", "O-O", "d5"},
                new List<string> {"d4", "Nf6", "c4", "g6", "Nc3", "Bg7", "e4", "d6", "Nf3", "O-O", "Be2", "e5", "O-O", "Nc6", "d5", "Ne7", "b4"}
            };
            openingIndex = 0;
        }

        private (string, bool) CheckOpening(List<string> moves)
        {
            for(int i = openingIndex; i < openings.Count; i++)
            {
                for(int j = 0; j < moves.Count; j++)
                {
                    if(openings[i].Count <= moves.Count && openings[i][j] != moves[j])
                    {
                        break;
                    }
                    if(j == moves.Count - 1 && openings[i].Count > moves.Count)
                    {
                        openingIndex = i;
                        return (openings[i][j+1], true);
                    }
                }
            }
            return ("", false);
        }
    }
}