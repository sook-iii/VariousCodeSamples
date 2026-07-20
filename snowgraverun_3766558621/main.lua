local snowgraveMod = RegisterMod("Snowgrave Run", 1)

-- Pseudocode for design goals since this is my first Isaac mod
-- When we pick up freeze items (Uranus, Freezer Baby, Cube Baby - exempts other random freezes because they won't be frequent through the remainder of a run) activate a different mode (you could even call it ... a weird root ... )
-- While this mode is active, swap default and boss music with WELCOME TO THE CITY. Maybe slow music down a la stopwatch effects? 
-- Every room, when the first enemy is frozen, play the weird route jingle
-- Every room clear, if an enemy was frozen, play the "get stronger" sfx

local primedForFirstFreeze = false
local primedForGettingStronger = false

local function CheckForFreezeItems()

    --print("Snowgrave: Starting Freeze Item Scan ... ")
	for PlrCount = 0, Game():GetNumPlayers() do 

		local plr = Isaac.GetPlayer(PlrCount)
		if plr:HasCollectible(596) or plr:HasCollectible(608) or plr:HasCollectible(652) then

            --print(" ... Freeze Item Found!")
			return true

		end
	end
    
    --print(" ... No Such Items.")
    return false

end

local function ScanForFrozenEnemies()

    --print("Snowgrave: Starting Frozen Enemy Scan ... ")
    for i, entity in ipairs(Isaac.GetRoomEntities()) do

        if entity.type == EntityType.ENEMY and entity:HasEntityFlags(EntityFlag.FLAG_ICE_FROZEN) then

            --print(" ... Frozen Enemy Found!")
            return true

        end
    end

    --print(" ... No Such Enemies.")
    return false

end

local function Proceed()

    --print("Snowgrave: Proceed")
    primedForGettingStronger = true    
    SFXManager():Play(Isaac.GetSoundIdByName("Weird_Route_Jingle"), 1, 0, false, 1)
    primedForFirstFreeze = false

end

local function IsaacBecomesStronger()

    --print("Snowgrave: Isaac Became Stronger")
    SFXManager():Play(Isaac.GetSoundIdByName("Became_Stronger"), 1, 0, false, 1)
    primedForGettingStronger = false

end

local function OnRoomEnter()

    --print("Snowgrave: New Room Entered")
    if CheckForFreezeItems() then 
        
        primedForFirstFreeze = true
        primedForGettingStronger = false

    end
end

local function OnUpdate() 

    if CheckForFreezeItems() then 

        if primedForFirstFreeze then

            if ScanForFrozenEnemies() then

                Proceed()

            end
        end
    end
end

local function OnRoomClear()

    --print("Snowgrave: Room Cleared")
    if CheckForFreezeItems() then 
            
        if primedForGettingStronger then

            IsaacBecomesStronger()

        end

        if Game():GetRoom():GetType() == RoomType.ROOM_BOSS then
           
            Game():GetHUD():ShowFortuneText("Isaac became stronger.")
            
        end
    end
end

local function OnPickup()

    --print("Snowgrave: Picked Up Item")
    if CheckForFreezeItems() then 

        musicID = MusicManager():GetCurrentMusicID()
        if musicID == Music.MUSIC_PLANETARIUM or musicID == Music.MUSIC_BASEMENT or musicID == Music.MUSIC_CAVES or musicID == Music.MUSIC_DEPTHS or musicID == Music.MUSIC_CELLAR or musicID == Music.MUSIC_CATACOMBS or musicID == Music.MUSIC_NECROPOLIS or musicID == Music.MUSIC_WOMB_UTERO or musicID == Music.MUSIC_CATHEDRAL or musicID == Music.MUSIC_SHEOL or musicID == Music.MUSIC_DARK_ROOM or musicID == Music.MUSIC_CHEST or musicID == Music.MUSIC_BURNING_BASEMENT or musicID == Music.MUSIC_FLOODED_CAVES or musicID == Music.MUSIC_DANK_DEPTHS or musicID == Music.MUSIC_SCARRED_WOMB or musicID == Music.MUSIC_BLUE_WOMB or musicID == Music.MUSIC_UTERO or musicID == Music.MUSIC_VOID or musicID == Music.MUSIC_DOWNPOUR or musicID == Music.MUSIC_MINES or musicID == Music.MUSIC_MAUSOLEUM or musicID == Music.MUSIC_CORPSE or musicID == Music.MUSIC_DROSS or musicID == Music.MUSIC_ASHPIT or musicID == Music.MUSIC_GEHENNA or musicID == Music.MUSIC_MORTIS or musicID == Music.MUSIC_DOWNPOUR_REVERSE or musicID == Music.MUSIC_DROSS_REVERSE then

            --print("Snowgrave: Slowing Things Down")
            MusicManager():Play(Isaac.GetMusicIdByName("Welcome_To_The_City"))
            MusicManager():UpdateVolume()

        end
    end
end

local function OnMusicPlay(table, musicID)
    
    --print("Snowgrave: New Music Playing")
    print(musicID)
    if musicID == Music.MUSIC_PLANETARIUM or musicID == Music.MUSIC_BASEMENT or musicID == Music.MUSIC_CAVES or musicID == Music.MUSIC_DEPTHS or musicID == Music.MUSIC_CELLAR or musicID == Music.MUSIC_CATACOMBS or musicID == Music.MUSIC_NECROPOLIS or musicID == Music.MUSIC_WOMB_UTERO or musicID == Music.MUSIC_CATHEDRAL or musicID == Music.MUSIC_SHEOL or musicID == Music.MUSIC_DARK_ROOM or musicID == Music.MUSIC_CHEST or musicID == Music.MUSIC_BURNING_BASEMENT or musicID == Music.MUSIC_FLOODED_CAVES or musicID == Music.MUSIC_DANK_DEPTHS or musicID == Music.MUSIC_SCARRED_WOMB or musicID == Music.MUSIC_BLUE_WOMB or musicID == Music.MUSIC_UTERO or musicID == Music.MUSIC_VOID or musicID == Music.MUSIC_DOWNPOUR or musicID == Music.MUSIC_MINES or musicID == Music.MUSIC_MAUSOLEUM or musicID == Music.MUSIC_CORPSE or musicID == Music.MUSIC_DROSS or musicID == Music.MUSIC_ASHPIT or musicID == Music.MUSIC_GEHENNA or musicID == Music.MUSIC_MORTIS then

        if CheckForFreezeItems() then 

            --print("Snowgrave: Slowing Things Down")
            return Isaac.GetMusicIdByName("Welcome_To_The_City")

        end
    end

    if musicID == Music.MUSIC_BOSS or musicID == Music.MUSIC_BOSS2 or musicID == Music.MUSIC_BOSS3 then

        if CheckForFreezeItems() then 

            --print("Snowgrave: Slowing Things Down")
            
            return Isaac.GetMusicIdByName("Berdly_Battle")

        end
    end

    if musicID == Music.MUSIC_DOWNPOUR_REVERSE or musicID == Music.MUSIC_DROSS_REVERSE then

        if CheckForFreezeItems() then 

            --print("Snowgrave: Slowing Things Down")
            
            return Isaac.GetMusicIdByName("Happy_Town")

        end
    end

    if musicID == Music.MUSIC_ISAACS_HOUSE then

        if CheckForFreezeItems() then 

            --print("Snowgrave: Slowing Things Down")
            
            return Isaac.GetMusicIdByName("Insert_Disk")

        end
    end
end

snowgraveMod:AddCallback(ModCallbacks.MC_POST_NEW_ROOM, OnRoomEnter)
snowgraveMod:AddCallback(ModCallbacks.MC_POST_UPDATE, OnUpdate)
snowgraveMod:AddCallback(ModCallbacks.MC_PRE_SPAWN_CLEAN_AWARD, OnRoomClear)

if REPENTOGON then
    snowgraveMod:AddCallback(ModCallbacks.MC_POST_ADD_COLLECTIBLE, OnPickup)
    snowgraveMod:AddCallback(ModCallbacks.MC_PRE_MUSIC_PLAY, OnMusicPlay)
end


