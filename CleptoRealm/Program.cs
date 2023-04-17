using System.Drawing;
class program 
{
    public static void GenerateLevelFromSeed(int seed, int again = -1)
    {
        g.levels[g.level].hiddencount = 0;
        g.levels[g.level].changes.Clear();
        g.levels[g.level].maproom = new Point();
        g.levels[g.level].chests.Clear();
        g.levels[g.level].chestcont.Clear();
        g.levels[g.level].map = new Room[g.roomspan + 1, g.roomspan + 1];
        g.current = new Point((g.plx - 1) / g.roomsize, (g.ply - 1) / g.roomsize);
        g.rooms = new Room[g.roomspan+1, g.roomspan+1];

        // e w n s
        int iter = 14;
        Random r = new Random(seed);
        if(r.Next(10) == 0) g.levels[g.level].mapt = true;
        g.rooms[0, 0] = new Room(1, 0, 0, 1);
        List<Point> points = new List<Point>();
        List<string> types = new List<string>();
        points.Add(new Point(0, 1));
        points.Add(new Point(1, 0));
        types.Add("plain");
        types.Add("plain");
        for (int i = 0; i <= g.roomspan; i++)
        {
            for (int j = 0; j <= g.roomspan; j++)
            {
                if (r.Next(20) == 0 && new Point(i, j) != new Point(0, 0) && new Point(i, j) != new Point(0, 1) && new Point(i, j) != new Point(1, 0)) g.rooms[i, j] = new Room();
            }
        }
        for (int k = 0; k <= iter; k++)
        {
            List<Point> bpoints = new List<Point>();
            List<string> btypes = new List<string>();
            //Console.WriteLine("-"+k);
            for (int i = 0; i <= points.Count - 1; i++)
            {
                //Console.WriteLine(i);
                int x = points[i].X;
                int y = points[i].Y;
                int e = 0;
                int n = 0;
                int s = 0;
                int w = 0;
                if (x != 0) 
                {
                    if (g.rooms[x - 1, y] == null && k != iter) w = r.Next(3);
                    else if (g.rooms[x - 1, y] != null && x != 0 && g.rooms[x - 1, y].E != null && ((g.rooms[x-1, y].type == "hidden" && types[i] == "hidden") || (g.rooms[x-1, y].type == "plain" && types[i] == "plain") || (g.rooms[x-1, y].type == "egress" && types[i] == "hidden"))) w = 1;
                }
                if (x != g.roomspan) 
                {
                    if (g.rooms[x + 1, y] == null && k != iter) e = r.Next(3);
                    else if (g.rooms[x + 1, y] != null && x != g.roomspan && g.rooms[x + 1, y].W != null && ((g.rooms[x+1, y].type == "hidden" && types[i] == "hidden") || (g.rooms[x+1, y].type == "plain" && types[i] == "plain") || (g.rooms[x+1, y].type == "egress" && types[i] == "hidden"))) e = 1;
                }
                if (y != 0)
                {
                    if (g.rooms[x, y - 1] == null && k != iter) n = r.Next(3);
                    else if (y != 0 && g.rooms[x, y - 1] != null && y != 0 && g.rooms[x, y - 1].S != null && ((g.rooms[x, y-1].type == "hidden" && types[i] == "hidden") || (g.rooms[x, y-1].type == "plain" && types[i] == "plain") || (g.rooms[x, y-1].type == "egress" && types[i] == "hidden"))) n = 1;
                }
                if (y != g.roomspan)
                {
                    if (g.rooms[x, y + 1] == null && k != iter) s = r.Next(3);
                    else if (g.rooms[x, y + 1] != null && y != g.roomspan && g.rooms[x, y + 1].N != null && (((g.rooms[x, y+1].type == "hidden" && types[i] == "hidden") || (g.rooms[x, y+1].type == "plain" && types[i] == "plain") || (g.rooms[x, y+1].type == "egress" && types[i] == "hidden")))) s = 1;
                }
                if (g.rooms[x, y] == null)
                {
                    g.rooms[x, y] = new Room(e, w, n, s);
                    if (r.Next(7) == 0 || k == 0) g.levels[g.level].exitroom = new Point(x, y);
                    if (r.Next(720) == 0 || i != 0 && new Point(x, y) != g.levels[g.level].exitroom && new Point(x, y) != g.levels[g.level].keyroom) g.levels[g.level].maproom = new Point(x, y);
                    if (types[i] == "hidden") g.rooms[x, y].type = "hidden";
                    if (((r.Next(12) == 0 && k >= 7) || k == 0) && g.rooms[x, y].type != "egress" && g.rooms[x, y].type != "hidden") g.levels[g.level].keyroom = new Point(x, y);
                    else if (r.Next(40) == 0 && k >= 7 && types[i] != "hidden") g.rooms[x, y].type = "egress";
                    //Console.WriteLine($"{x}, {y}, n:{rooms[x, y].n}, s:{rooms[x, y].s}, e:{rooms[x, y].e}, w:{rooms[x, y].w}");
                    if (x != 0 && g.rooms[x, y].W != null && g.rooms[x - 1, y] == null && !bpoints.Contains(new Point(x - 1, y)))
                    {
                        bpoints.Add(new Point(x - 1, y));
                        if (g.rooms[x, y].type == "hidden" || g.rooms[x, y].type == "egress") btypes.Add("hidden");
                        else btypes.Add("plain");
                    }
                    if (x != g.roomspan && g.rooms[x, y].E != null && g.rooms[x + 1, y] == null && !bpoints.Contains(new Point(x + 1, y)))
                    {
                        bpoints.Add(new Point(x + 1, y));
                        if (g.rooms[x, y].type == "hidden" || g.rooms[x, y].type == "egress") btypes.Add("hidden");
                        else btypes.Add("plain");
                    }
                    if (y != 0 && g.rooms[x, y].N != null && g.rooms[x, y - 1] == null && !bpoints.Contains(new Point(x, y - 1)))
                    {
                        bpoints.Add(new Point(x, y - 1));
                        if (g.rooms[x, y].type == "hidden" || g.rooms[x, y].type == "egress") btypes.Add("hidden");
                        else btypes.Add("plain");
                    }
                    if (y != g.roomspan && g.rooms[x, y].S != null && g.rooms[x, y + 1] == null && !bpoints.Contains(new Point(x, y + 1)))
                    {
                        bpoints.Add(new Point(x, y + 1));
                        if (g.rooms[x, y].type == "hidden" || g.rooms[x, y].type == "egress") btypes.Add("hidden");
                        else btypes.Add("plain");
                    }
                }
            }
            points = bpoints;
            types = btypes;
            //Console.WriteLine(points.Count);
        }
        for (int i = 0; i <= g.roomspan; i++)
        {
            for (int j = 0; j <= g.roomspan; j++)
            {
                if (g.rooms[i, j] != null)
                {
                    switch (g.rooms[i, j].type) 
                    {
                        case "egress":
                            g.levels[g.level].hiddencount++;
                            if (i != 0 && g.rooms[i - 1, j] != null && g.rooms[i - 1, j].type == "plain") 
                            {
                                g.rooms[i-1, j].SetE("lock");
                                g.rooms[i, j].SetW("lock");
                            }
                            if (i != g.roomspan && g.rooms[i+1, j] != null && g.rooms[i+1, j].type == "plain")
                            {
                                g.rooms[i+1, j].SetW("lock");
                                g.rooms[i, j].SetE("lock");
                            }
                            if (j != 0 && g.rooms[i, j-1] != null && g.rooms[i, j-1].type == "plain")
                            {
                                g.rooms[i, j-1].SetS("lock");
                                g.rooms[i, j].SetN("lock");
                            }
                            if (j != g.roomspan && g.rooms[i, j+1] != null && g.rooms[i, j+1].type == "plain")
                            {
                                g.rooms[i, j+1].SetN("lock");
                                g.rooms[i, j].SetS("lock");
                            }
                            break;
                        case "hidden":
                            if (i != 0 && g.rooms[i-1, j] != null && g.rooms[i-1, j].type != "hidden" && g.rooms[i-1, j].type != "egress")
                            {
                                g.rooms[i-1, j].E = null;
                                g.rooms[i, j].W = null;
                            }
                            if (i != g.roomspan && g.rooms[i+1, j] != null && g.rooms[i+1, j].type != "hidden" && g.rooms[i+1, j].type != "egress")
                            {
                                g.rooms[i+1, j].W = null;
                                g.rooms[i, j].E = null;
                            }
                            if (j != 0 && g.rooms[i, j-1] != null && g.rooms[i, j-1].type != "hidden" && g.rooms[i, j-1].type != "egress")
                            {
                                g.rooms[i, j-1].S = null;
                                g.rooms[i, j].N = null;
                            }
                            if (j != g.roomspan && g.rooms[i, j+1] != null && g.rooms[i, j+1].type != "hidden" && g.rooms[i, j+1].type != "egress")
                            {
                                g.rooms[i, j+1].N = null;
                                g.rooms[i, j].S = null;
                            }
                            break;
                    }
                }
            }
        }
        for (int i = 0; i <= g.roomspan; i++)
        {
            for (int j = 0; j <= g.roomspan; j++)
            {
                int x = r.Next(g.roomsize - 2);
                int y = r.Next(g.roomsize - 2);
                for (int k = 0; k <= g.roomsize - 1; k++)
                {
                    for (int l = 0; l <= g.roomsize - 1; l++)
                    {
                        if ((i == 0 && k == 0) || (j == 0 && l == 0)) g.loaded[i * g.roomsize + k, j * g.roomsize + l] = 2;
                        if (g.rooms[j, i] == null || g.rooms[j, i].type == "obst") g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 2;
                        else
                        {
                            if (k == g.roomsize - 1 || l == g.roomsize - 1)
                            {
                                if ((k == x && g.rooms[j, i].E != null) || (l == y && g.rooms[j, i].S != null))
                                {
                                    if ((k == x && g.rooms[j, i].E != null && g.rooms[j, i].E.type == "lock") || (l == y && g.rooms[j, i].S != null && g.rooms[j, i].S.type == "lock")) g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 9;
                                    else
                                    {
                                        if (r.Next(7) == 0) g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 7;
                                        else g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 6;
                                    }
                                }
                                else g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 2;
                            }
                            else
                            {
                                if (r.Next(30) == 0)
                                {
                                    int rand = r.Next(30);
                                    if (rand == 0 || g.rooms[j, i].type == "egress") g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 5;
                                    else if (((Between(rand, 1, 7) && g.rooms[j, i].type == "egress") || (Between(rand, 16, 28) && g.rooms[j, i].type == "hidden")) && (i != 0 && j != 0)) g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 8;
                                    else if (Between(rand, 8, 12)) g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 18;
                                    else if (Between(rand, 13, 15)) g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 12;
                                    else g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 19;
                                }
                                else g.loaded[i * g.roomsize + k + 1, j * g.roomsize + l + 1] = 0;
                            }
                        }
                    }
                }
            }
        }
        g.loaded[2, 2] = 3;
        g.loaded[g.levels[g.level].exitroom.Y * g.roomsize + 1 + 1, g.levels[g.level].exitroom.X * g.roomsize + 1 + 1] = 4;
        if(g.levels[g.level].mapt) g.loaded[g.levels[g.level].maproom.Y * g.roomsize + 1 + 1, g.levels[g.level].maproom.X * g.roomsize + 1 + 1] = 22;
        if(g.levels[g.level].hiddencount != 0) g.loaded[g.levels[g.level].keyroom.Y * g.roomsize + 1 + 1, g.levels[g.level].keyroom.X * g.roomsize + 1 + 1] = 17;
        //if(g.debug) g.loaded[3, 1] = 21;
        if (again != -1) 
        {
            for (int i = 0; i <= g.levels[again].changes.Count-1; i++) 
            {
                g.loaded[g.levels[again].changes[i].x, g.levels[again].changes[i].y] = g.levels[again].changes[i].index;
            }
        }
    }
    public static void UseItem(Point p) 
    {
        Random r = new Random();
        int x = p.X;
        int y = p.Y;
        switch (g.inventory[g.invsel].id)
        {
            case 3: Get(4); break;
            case 4:
                if (g.loaded[g.plx + x, g.ply + y] == 18)
                {
                    g.loaded[g.plx + x, g.ply + y] = 19;
                    g.levels[g.level].AddChange(g.plx + x, g.ply + y, 19);
                    g.inventory[g.invsel] = new Thing(65);
                }
                break;
            case 36:
                if (g.loaded[g.plx + x, g.ply + y] == 18)
                {
                    g.loaded[g.plx + x, g.ply + y] = 19;
                    g.levels[g.level].AddChange(g.plx + x, g.ply + y, 19);
                }
                break;
            case 8:
                if (g.loaded[g.plx + x, g.ply + y] == 9 && g.inventory[g.invsel].cint == g.level) 
                {
                    g.loaded[g.plx + x, g.ply + y] = 7;
                    g.levels[g.level].AddChange(g.plx + x, g.ply + y, 7);
                }
                break;
        }
    }
    public static int FillChest() 
    {
        Random r = new Random();
        int[] forb = {0, 8 };
        int ret = 0;
        while(forb.Contains(ret)) 
        {
            ret = r.Next(Items.count);
        }
        return ret;
    }
    public static string Gap(int size) 
    {
        string s = "";
        for (int i = 1; i<=size; i++) 
        {
            s += " ";
        }
        return s;
    }
    public static void UpdateMap(Point room) 
    {
        g.levels[g.level].map[room.Y, room.X] = g.rooms[room.Y, room.X];
    }
    public static void UpdateUI() 
    {
        g.ui[0] = $"seed: {g.seed}";
        g.ui[1] = $"room X: {g.current.X} Y: {g.current.Y}";
        g.ui[2] = $"Level: {g.level}/{g.levels.Count-1}";
        g.ui[4] = $"{g.charname}";
        g.ui[5] = $"X: {g.plx} Y: {g.ply}";
        g.ui[6] = $"Direction: {g.direction}";
        if (LookingAt(g.plx, g.ply, g.direction) == 0) g.ui[7] = " ";
        else g.ui[7] = $"Looking at {Tiles.name[LookingAt(g.plx, g.ply, g.direction)]}";
        g.ui[9] = "---Inventory";
        for (int i = 0; i <= g.visinv; i++)
        {
            if (g.invsel == i)
            {
                if (g.inventory[i].id != 0) g.ui[10 + i] = $">>>[{i}] " + g.inventory[i].name + " | Press i for info    ";
                else g.ui[10 + i] = $">>>[{i}] " + g.inventory[i].name+"                 ";
            }
            else g.ui[10 + i] = $"   [{i}] " + g.inventory[i].name;
        }
        if (g.debug) g.ui[g.ui.Length - 1] = "!DEBUG MODE ON!";
        else g.ui[g.ui.Length - 1] = g.lineempty;
        for (int i = 0; i<g.size-1; i++) 
        {
            Typein(g.size * 2 + g.borderoffset.Length + 4, i + 2, g.ui[i] + g.lineempty);
        }
    }
    public static bool NotFull() 
    {
        int i = 0;
        while (i <= g.inventory.Length - 1 && g.inventory[i].id != 0)
        {
            i++;
        }
        if (i <= g.inventory.Length - 1) return true;
        else return false;
    }
    public static void Screen(int type) 
    {
        for (int i = 0; i < g.size - 1; i++)
        {
            for (int j = 0; j < g.size - 1; j++)
            {
                Typein((i * 2) + 4, j + 2, "  ");
            }
        }
        switch (type) 
        {
            case 0:
                Typein(0, g.size + 2, g.clearline);
                Typein(0, g.size + 3, g.clearline);
                Typein(0, g.size + 3, "Press any button...");
                Typein(4, g.middle, Gap((((g.size-1)*2)-23)/2)+"You've submerged deeper");
                Typein(4, g.middle+1, Gap((((g.size-1)*2)-21)/2)+"into the underground…");
                Console.SetCursorPosition(0, g.size + 2);
                Console.ReadKey();
                break;
            case 1:
                char anw = ' ';
                int sel = 0;
                Random r = new Random();
                int[] contents = new int[10];
                if (g.levels[g.level].chests.Contains(new Point(g.plx + D(g.direction).X, g.ply + D(g.direction).Y))) contents = g.levels[g.level].chestcont[g.levels[g.level].chests.IndexOf(new Point(g.plx + D(g.direction).X, g.ply + D(g.direction).Y))];
                else 
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        if (r.Next(3) == 0) contents[i] = FillChest();
                        else contents[i] = 0;
                    }
                }
                Typein(0, g.size + 3, "ws - chest, ol - inventory, e - take, z - put, x - exit");
                Typein(4, 2, "---Chest");
                while (anw != 'x') 
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        if (i == sel) Typein(4, 3 + i, $">>>[{i}] " + Items.name[contents[i]]+"               ");
                        else Typein(4, 3 + i, $"   [{i}] " + Items.name[contents[i]]+"                ");
                    }
                    UpdateUI();
                    Console.SetCursorPosition(0, g.size + 2);
                    anw = Console.ReadKey().KeyChar;
                    switch (anw)
                    {
                        case 's':
                            if (sel == 9) sel = 0;
                            else sel++;
                            break;
                        case 'w':
                            if (sel == 0) sel = 9;
                            else sel--;
                            break;
                        case 'e':
                            if (NotFull()) 
                            {
                                Get(contents[sel]);
                                contents[sel] = 0;
                            }
                            break;
                        case 'l':
                            if (g.invsel != 7) g.invsel++;
                            else g.invsel = 0;
                            break;
                        case 'o':
                            if (g.invsel != 0) g.invsel--;
                            else g.invsel = 7;
                            break;
                        case 'z':
                            if (g.inventory[g.invsel].id != 0 && contents[sel] == 0) 
                            {
                                contents[sel] = g.inventory[g.invsel].id;
                                g.inventory[g.invsel] = new Thing();
                            }
                            break;
                    }
                }
                if (!g.levels[g.level].chests.Contains(new Point(g.plx + D(g.direction).X, g.ply + D(g.direction).Y))) 
                {
                    g.levels[g.level].chests.Add(new Point(g.plx + D(g.direction).X, g.ply + D(g.direction).Y));
                    g.levels[g.level].chestcont.Add(contents);
                }
                break;
            case 2:
                //"WASD - look around, wasd - walk, R - generate seed, e - interact, ol - inventory, z - drop item"
                Typein(0, g.size + 2, g.clearline);
                Typein(0, g.size + 3, g.clearline);
                Typein(0, g.size + 3, "Press any button...");
                Typein(5, g.middle, "WASD - look around");
                Typein(5, g.middle + 1, "wasd - walk");
                Typein(5, g.middle + 2, "R - generate seed");
                Typein(5, g.middle + 3, "e - interact");
                Typein(5, g.middle + 4, "ol - inventory");
                Typein(5, g.middle + 5, "z - drop item");
                Typein(5, g.middle + 5, "m - map");
                Typein(5, g.middle + 6, "f - use item");
                Console.SetCursorPosition(0, g.size + 2);
                Console.ReadKey();
                break;
            case 3:
                for (int i = 0; i < (g.size)/2; i++)
                {
                    for (int j = 0; j < (g.size - 1); j++)
                    {
                        if (g.rooms[i, j] != null && g.rooms[i, j].type != "obst") 
                        {
                            if (new Point(i, j) == new Point(g.current.Y, g.current.X)) 
                            {
                                Console.ForegroundColor = Tiles.color[1];
                                Typein((j * 2) + 4, (i * 2) + 2, Tiles.tile[1].ToString());
                                Console.ResetColor();
                            }
                            else if(new Point(i, j) == g.levels[g.level].exitroom) 
                            {
                                Console.ForegroundColor = Tiles.color[4];
                                Typein((j * 2) + 4, (i * 2) + 2, Tiles.tile[4].ToString());
                                Console.ResetColor();
                            }
                            else if (new Point(i, j) == g.levels[g.level].maproom && g.levels[g.level].mapt)
                            {
                                Console.ForegroundColor = Tiles.color[22];
                                Typein((j * 2) + 4, (i * 2) + 2, Tiles.tile[22].ToString());
                                Console.ResetColor();
                            }
                            else if (new Point(i, j) == new Point(0, 0)) 
                            {
                                Console.ForegroundColor = Tiles.color[3];
                                Typein((j * 2) + 4, (i * 2) + 2, Tiles.tile[3].ToString());
                                Console.ResetColor();
                            }
                            else if (g.rooms[i, j].type == "hidden" || g.rooms[i, j].type == "egress") Typein((j * 2) + 4, (i * 2) + 2, "■");
                            else Typein((j * 2) + 4, (i * 2) + 2, "□");
                            if (g.rooms[i, j].S != null) 
                            {
                                if( g.rooms[i, j].S.type == "lock") Typein((j * 2) + 5, (i * 2) + 2, "≠");
                                else Typein((j * 2) + 5, (i * 2) + 2, "=");
                            }
                            if (g.rooms[i, j].E != null && i < (g.size / 2) - 1) 
                            {
                                if (g.rooms[i, j].E.type == "lock") Typein((j * 2) + 4, (i * 2) + 3, "╫");
                                else Typein((j * 2) + 4, (i * 2) + 3, "‖");
                            }
                        } 
                    }
                }
                Typein(0, g.size + 2, g.clearline);
                Typein(0, g.size + 3, g.clearline);
                Typein(0, g.size + 3, "Press any button...");
                Console.ReadKey();
                break;
            case 4:
                for (int i = 0; i < (g.size) / 2; i++)
                {
                    for (int j = 0; j < (g.size - 1); j++)
                    {
                        if (g.levels[g.level].map[i, j] != null && g.levels[g.level].map[i, j].type != "obst")
                        {
                            if (new Point(i, j) == new Point(g.current.Y, g.current.X))
                            {
                                Console.ForegroundColor = Tiles.color[1];
                                Typein((j * 2) + 4, (i * 2) + 2, Tiles.tile[1].ToString());
                                Console.ResetColor();
                            }
                            else if (new Point(i, j) == g.levels[g.level].exitroom)
                            {
                                Console.ForegroundColor = Tiles.color[4];
                                Typein((j * 2) + 4, (i * 2) + 2, Tiles.tile[4].ToString());
                                Console.ResetColor();
                            }
                            else if (new Point(i, j) == g.levels[g.level].maproom && g.levels[g.level].mapt)
                            {
                                Console.ForegroundColor = Tiles.color[22];
                                Typein((j * 2) + 4, (i * 2) + 2, Tiles.tile[22].ToString());
                                Console.ResetColor();
                            }
                            else if (new Point(i, j) == new Point(0, 0))
                            {
                                Console.ForegroundColor = Tiles.color[3];
                                Typein((j * 2) + 4, (i * 2) + 2, Tiles.tile[3].ToString());
                                Console.ResetColor();
                            }
                            else if(g.rooms[i, j].type == "hidden" || g.rooms[i, j].type == "egress") Typein((j * 2) + 4, (i * 2) + 2, "■");
                            else Typein((j * 2) + 4, (i * 2) + 2, "□");
                            if (g.rooms[i, j].S != null)
                            {
                                if (g.rooms[i, j].S.type == "lock") Typein((j * 2) + 5, (i * 2) + 2, "≠");
                                else Typein((j * 2) + 5, (i * 2) + 2, "=");
                            }
                            if (g.rooms[i, j].E != null && i < (g.size / 2) - 1)
                            {
                                if (g.rooms[i, j].E.type == "lock") Typein((j * 2) + 4, (i * 2) + 3, "╫");
                                else Typein((j * 2) + 4, (i * 2) + 3, "‖");
                            }
                        }
                    }
                }
                Typein(0, g.size + 2, g.clearline);
                Typein(0, g.size + 3, g.clearline);
                Typein(0, g.size + 3, "Press any button...");
                Console.ReadKey();
                break;
        }
    }
    public static void Get(int index) 
    {
        int i = 0;
        while (i <= g.inventory.Length - 1 && g.inventory[i].id != 0)
        {
            i++;
        }
        if (i <= g.inventory.Length - 1)
        {
            g.inventory[i] = new Thing(index);
        }
    }
    public static void LevelUp()
    {
        g.seed = g.primaryseed;
        for (int i = 0; i<=g.level-2; i++) 
        {
            Random r = new Random(g.seed);
            g.seed = r.Next();
        }
        GenerateLevelFromSeed(g.seed, g.level-1);
        Teleport((g.levels[g.level].exitroom.Y*g.roomsize)+3, (g.levels[g.level].exitroom.X * g.roomsize) + 2);
        g.level--;
    }
    public static void LevelDown() 
    {
        Random r = new Random(g.seed);
        g.seed = r.Next();
        if (g.level == g.levels.Count - 1) 
        {
            Screen(0);
            g.levels.Add(new Level());

        } 
        g.level++;
        GenerateLevelFromSeed(g.seed);
        Teleport(3, 2);
    }
    public static bool InventoryContains(int index) 
    {
        bool b = false;
        for (int i = 0; i<= g.inventory.Length-1; i++) 
        {
            if (g.inventory[i].id == index) b = true;
        }
        return b;
    }
    public static Point D(char dir) 
    {
        switch (dir)
        {
            case 'N': return new Point(0, -1); break;
            case 'S': return new Point(0, 1); break;
            case 'E': return new Point(1, 0); break;
            case 'W': return new Point(-1, 0); break;
            default: return new Point(0, 1); break;
        }
    }
    public static void Interact(Point p) 
    {
        Random r = new Random();
        int x = p.X;
        int y = p.Y;
        switch (g.loaded[g.plx+x, g.ply+y])
        {
            case 3: if(g.level != 0) LevelUp(); break;
            case 4: LevelDown(); break;
            case 6: g.loaded[g.plx + x, g.ply + y] = 7; g.levels[g.level].AddChange(g.plx + x, g.ply + y, 7); break;
            case 7: g.loaded[g.plx + x, g.ply + y] = 6; g.levels[g.level].AddChange(g.plx + x, g.ply + y, 6); break;
            case 8:
                Screen(1);
                //g.loaded[g.plx + x, g.ply + y] = 20;
                break;
            case 9:
                g.message = "You need a key to open this door";
                break;
            case 18:
                PickUp(p);
                break;
            case 19:
                g.levels[g.level].AddChange(g.plx + x, g.ply + y, 18);
                g.loaded[g.plx + x, g.ply + y] = 18;
                g.message = "You put out the candle";
                break;
            case 21: Get(r.Next(Items.count)); break;
            case 22: Screen(3); break;
            default: PickUp(p); break;
        }
    }
    public static void PickUp(Point p) 
    {
        int i = 0;
        while (i <= g.inventory.Length-1 && g.inventory[i].id != 0) 
        {
            i++;
        }
        if (i <= g.inventory.Length - 1) 
        {
            if (Tiles.item[g.loaded[g.plx + p.X, g.ply + p.Y]] == 8) g.inventory[i] = new Thing(Tiles.item[g.loaded[g.plx + p.X, g.ply + p.Y]], g.level);
            else g.inventory[i] = new Thing(Tiles.item[g.loaded[g.plx + p.X, g.ply + p.Y]]);
            if (Tiles.item[g.loaded[g.plx + p.X, g.ply + p.Y]] != 0) 
            {
                g.loaded[g.plx + p.X, g.ply + p.Y] = 0;
                g.levels[g.level].AddChange(g.plx + p.X, g.ply + p.Y, 0);
            }
        }
    }
    public static bool Between(int i, int more, int less) 
    {
        if (i <= less && i >= more) return true;
        else return false;
    }
    public static void Typein(int x, int y, string txt) 
    {
        Console.SetCursorPosition(x, y);
        Console.Write(txt);
    }
    public static int LookingAt(int X, int Y, char dir) 
    {
        switch (dir) 
        {
            case 'N':
                if (Y != 0) return g.space[x(X), y(Y - 1)];
                else return 0;
                break;
            case 'E':
                return g.space[x(X+1), y(Y)];
                break;
            case 'W':
                if (X != 0) return g.space[x(X - 1), y(Y)];
                else return 0;
                break;
            case 'S':
            default:
                return g.space[x(X), y(Y+1)];
                break;
        }
    }
    public static void Move(int xi, int yi, char dir)
    {
        g.direction = dir;
        if (((g.plx > 0 && xi < 0) || (g.ply > 0 && yi < 0) || (xi >= 0 && yi >= 0)) && Tiles.passable[g.space[x(g.plx + xi), y(g.ply + yi)]])
        {
            g.plx += xi;
            g.ply += yi;
            if ((g.ply > g.middle-1 && yi < 0) || (g.ply > g.middle && yi > 0)) g.cameray += yi;
            if ((g.plx > g.middle - 1 && xi < 0) || (g.plx > g.middle && xi > 0)) g.camerax += xi;
        }
    }
    public static int x(int x)
    {
        return x + g.middle - g.camerax;
    }
    public static int y(int y)
    {
        return y + g.middle - g.cameray;
    }
    public static int X(int x) 
    {
        return x - g.middle + g.camerax;
    }
    public static int Y(int y)
    {
        return y - g.middle + g.cameray;
    }
    public static void Teleport(int x, int y) 
    {
        g.plx = x;
        g.ply = y;
        if (g.plx >= g.middle) g.camerax = g.plx;
        else g.camerax = g.middle;
        if (g.ply >= g.middle) g.cameray = g.ply;
        else g.cameray = g.middle;
    }
    public static void Command() 
    {
        Console.WriteLine(": Type in your command");
        Typein(0, g.size + 3, g.clearline);
        Console.SetCursorPosition(0, g.size + 3);
        string[] anw = Console.ReadLine().Split(' ');
        switch(anw[0].ToLower())
        {
            case "tp":
                try
                {
                    Teleport(Convert.ToInt32(anw[1]), Convert.ToInt32(anw[2]));
                    g.message = $"Succesfully teleported to {g.plx}, {g.ply}";
                }
                catch { g.message = "Invalid command"; }
                break;
            case "":
                g.message = "bruh";
                break;
            case "sex":
                g.message = "You get no bitches";
                break;
            case "tiletest":
                Typein(0, g.size + 3, g.clearline);
                Console.SetCursorPosition(0, g.size + 3);
                for (int i = 0; i <= Tiles.name.Count-1; i++) 
                {
                    Console.ForegroundColor = Tiles.color[i];
                    Console.Write(Tiles.tile[i]);
                }
                Console.ResetColor();
                Console.WriteLine("  Press any button to continue... ");
                Console.ReadKey();
                break;
            case "changename":
                Console.WriteLine(" Type in your new name: ");
                g.charname = Console.ReadLine();
                break;
            case "roomsize":
                Console.WriteLine(" Type in new room size: ");
                g.roomsize = int.Parse(Console.ReadLine());
                break;
            case "exit":
                g.message = "Exit";
                break;
            case "suck":
                g.message = "no u";
                break;
            case "useseed":
                Console.SetCursorPosition(0, g.size + 4);
                try { g.seed = int.Parse(anw[1]); }
                catch { g.message = "Invalid command"; }
                GenerateLevelFromSeed(g.seed);
                break;
            case "get":
                try { Get(int.Parse(anw[1])); }
                catch { g.message = "Invalid command"; }
                break;
            case "controls":
                Screen(2);
                break;
            default:
                g.message = $"Command '{anw[0]}' doesn't exist";
                Console.Beep();
                break;
        }
    }
    static void Main()
    {
        Console.Title = "Gaem prototype";
        Console.WindowHeight = g.size+7;
        Console.SetWindowPosition(0, 0);
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        /// introducing vars
        
        Tiles.OpenTable();
        Items.OpenTable();
        g.message = "c - to open console. type 'controls'";
        char anw = ' ';
        Random r = new Random();
        g.primaryseed = r.Next();
        g.seed = g.primaryseed;
        string udborder = "";
        char[,] view = new char[g.size, g.size];
        int[,] screenmark = new int[g.size, g.size];
        for (int i = 0; i<=g.inventory.Length-1; i++) 
        {
            g.inventory[i] = new Thing();
        }
        g.inventory[0] = new Thing(2);

        for (int i = 1; i<=g.size-1; i++) 
        {
            udborder += "--";
        }

        Typein(g.borderoffset.Length - 3, 1, g.borderoffset + "/" + udborder + @"\");
        for (int i = 0; i < g.size - 1; i++)
        {
            Typein(0, i + 2, g.borderoffset + "|");
            Typein(g.size * 2 + g.borderoffset.Length - 1, i + 2, "|   " + g.lineempty);
        }
        Typein(0, g.size + 1, g.borderoffset + @"\" + udborder + "/");

        g.levels.Add(new Level());
        GenerateLevelFromSeed(g.seed);

        while (g.message != "Exit")
        {
            Typein(0, g.size+2, g.clearline);
            Console.WriteLine();
            if (g.current != new Point((g.plx-1) / g.roomsize, (g.ply-1) / g.roomsize)) g.current = new Point((g.plx - 1) / g.roomsize, (g.ply - 1) / g.roomsize);
            UpdateMap(g.current);
            /// startpoint data

            for (int i = 0; i <= g.size - 1; i++)
            {
                for (int j = 0; j <= g.size - 1; j++)
                {
                    g.space[i, j] = 0;
                    screenmark[i, j] = 0;
                }
            }
            screenmark[x(g.plx), y(g.ply)] = 1;

            /// Seed-filling

            for (int i = 0; i < g.size; i++)
            {
                for (int j = 0; j < g.size; j++)
                {
                    g.space[i, j] = g.loaded[X(i), Y(j)];
                }
            }
            /// Character set

            for (int i = 0; i < g.size; i++)
            {
                for (int j = 0; j < g.size; j++)
                {
                    if(screenmark[i, j] == 0) view[i, j] = Tiles.tile[g.space[i, j]];
                    else view[i, j] = Tiles.tile[screenmark[i, j]];
                }
            }

            /// UI

            UpdateUI();
            
            //ui[4] = anw.ToString();

            /// Showing on screen

            for (int i = 0; i < g.size - 1; i++)
            {
                for (int j = 0; j < g.size - 1; j++)
                {
                    Console.ForegroundColor = Tiles.color[Tiles.tile.IndexOf(view[j, i])];
                    if (Tiles.solidtiling[Tiles.tile.IndexOf(view[j, i])]) Typein(j * 2 + g.borderoffset.Length + 1, i + 2, view[j, i].ToString());
                    else Typein(j * 2 + g.borderoffset.Length + 1, i + 2, " ");
                    Typein(j * 2 + g.borderoffset.Length + 2, i + 2, view[j, i].ToString());
                }
                Console.ResetColor();
                Typein(g.size * 2 + g.borderoffset.Length + 4, i + 2, g.ui[i] + g.lineempty);
            }
            Typein(0, g.size + 3, g.clearline);
            Typein(0, g.size + 3, g.message);
            Console.SetCursorPosition(0, g.size+2);
            anw = Console.ReadKey().KeyChar;
            switch (anw) 
            {
                case 'd':
                    Move(1, 0, 'E');
                    break;
                case 'a':
                    Move(-1, 0, 'W');
                    break;
                case 'w':
                    Move(0, -1, 'N');
                    break;
                case 's':
                    Move(0, 1, 'S');
                    break;
                case 'A':
                    g.direction = 'W';
                    break;
                case 'D':
                    g.direction = 'E';
                    break;
                case 'W':
                    g.direction = 'N';
                    break;
                case 'S':
                    g.direction = 'S';
                    break;
                case 'R':
                    Console.SetCursorPosition(0, g.size+4);
                    g.primaryseed = r.Next();
                    g.seed = g.primaryseed;
                    g.level = 0;
                    g.levels.Clear();
                    g.levels.Add(new Level());
                    GenerateLevelFromSeed(g.seed);
                    break;
                case 'r':
                    GenerateLevelFromSeed(g.seed);
                    break;
                case 'e': case 'E':
                    Interact(D(g.direction));
                    break;
                case 'f': case 'F':
                    UseItem(D(g.direction));
                    break;
                case 'c':
                    Command();
                    break;
                case 'l':
                    if (g.invsel != 7) g.invsel++;
                    else g.invsel = 0;
                    break;
                case 'o':
                    if (g.invsel != 0) g.invsel--;
                    else g.invsel = 7;
                    break;
                case 'z':
                    g.inventory[g.invsel] = new Thing();
                    break;
                case 'i':
                    if (g.inventory[g.invsel].id != 0) g.message = $"{Items.name[g.inventory[g.invsel].id]} - {Items.desc[g.inventory[g.invsel].id]}";
                    break;
                case 'm':
                    Screen(4);
                    break;
                case 'M':
                    Screen(3);
                    break;
            }
        }
    }
    public static class g 
    {
        public static List<Level> levels = new List<Level>();
        public static int level = 0;
        public static int primaryseed = 0;
        public static int visinv = 7;
        public static Point current = new Point(0, 0);
        public static int screen = 0;
        public static int invsel = 0;
        public static int roomspan = 50;
        public static int seed;
        public static bool debug = true;
        public static string lineempty = "                              ";
        public static string clearline = "                                                                                                      ";
        public static int loadedsize = 1000;
        public static bool key = false;
        public static int[,] loaded = new int[loadedsize, loadedsize];
        public static int roomsize = 4;
        public static int size = 22;
        public static string[] ui = new string[size - 1];
        public static int[,] space = new int[size, size];
        public static Room[,] rooms = new Room[100, 100];
        public static Thing[] inventory = new Thing[8];
        public static string borderoffset = "   ";
        public static string message = "";
        public static int[] unpassable = { 2, 6 };
        public static int plx = 3;
        public static int ply = 2;
        public static char direction = 'S';
        public static int camerax = 10;
        public static int cameray = 10;
        public static int middle = (size / 2) - 1;
        public static string charname = "Daddy's litle beta tester";
    }
    public class Level
    {
        public bool mapt = false;
        public int hiddencount = 0;
        public List<Change> changes = new List<Change>();
        public List<Point> chests = new List<Point>();
        public List<int[]> chestcont = new List<int[]>();
        public Point exitroom, keyroom, maproom = new Point();
        public Room[,] map = new Room[100, 100];
        public class Change
        {
            public int x, y, index;
            public Change(int x, int y, int i) 
            {
                this.x = x;
                this.y = y;
                this.index = i;
            }
        }
        public void AddChange(int x, int y, int i) 
        {
            this.changes.Add(new Change(x, y, i));
        }
    }
    public class Thing
    {
        public string name = "";
        public int id = 0;
        public string cstr = "";
        public bool cbool = false;
        public int cint = 0;
        public Thing(int id)
        {
            this.id = id;
            this.name = Items.name[id];
        }
        public Thing(int id, int cint)
        {
            this.id = id;
            this.cint = cint;
            if(id == 8) this.name = $"level {cint} "+Items.name[id];
            else this.name = Items.name[id];
        }
        public Thing() {}
    }
    public static class Items 
    {
        public static int count;
        public static List<string> name = new List<string>();
        public static List<string> desc = new List<string>();
        public static void OpenTable() 
        {
            List<string> data = new List<string>();
            data = File.ReadAllLines("Items.txt").ToList();
            int n = data.Count;
            for (int i = 1; i <= n - 1; i++)
            {
                List<string> row = new List<string>();
                row = data[i].Split(' ').ToList();

                name.Add(row[1].Replace('_', ' '));
                desc.Add(row[2].Replace('_', ' '));
            }
            count = name.Count;
        }
    }
    public static class Tiles 
    {
        public static List<string> name = new List<string>();
        public static List<char> tile = new List<char>();
        public static List<int> item = new List<int>();
        public static List<ConsoleColor> color = new List<ConsoleColor>();
        public static List<bool> passable = new List<bool>();
        public static List<bool> solidtiling = new List<bool>();
        public static void OpenTable() 
        {
            List<string> data = new List<string>();
            data = File.ReadAllLines("Tiles.txt").ToList();
            int n = data.Count;
            for (int i = 1; i <= n-1; i++) 
            {
                List<string> row = new List<string>();
                row = data[i].Split(' ').ToList();
                ConsoleColor c;
                Enum.TryParse(row[4], out c);

                name.Add(row[1].Replace('_', ' '));
                tile.Add(row[2][0]);
                item.Add(int.Parse(row[3]));
                color.Add(c);
                passable.Add(bool.Parse(row[5]));
                solidtiling.Add(bool.Parse(row[6]));
            }
        }
    }
    public class Room 
    {
        public string type = "plain";
        public Direction E, W, S, N = new Direction();
        public Room(int e, int w, int n, int s) 
        {
            if (e != 0) E = new Direction("plain");
            if (w != 0) W = new Direction("plain");
            if (n != 0) N = new Direction("plain");
            if (s != 0) S = new Direction("plain");
        }
        public Room()
        {
            type = "obst";
        }
        public void SetE(string type) 
        {
            E = new Direction(type);
        }
        public void SetW(string type)
        {
            W = new Direction(type);
        }
        public void SetS(string type)
        {
            S = new Direction(type);
        }
        public void SetN(string type)
        {
            N = new Direction(type);
        }
        internal class Direction
        {
            public string type = "plain";
            internal Direction(string type = "plain")
            {
                this.type = type;
            }
            
        }
    }
}