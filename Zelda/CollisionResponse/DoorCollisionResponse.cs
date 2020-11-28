﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using game_project.ECS;
using game_project.ECS.Components;
using game_project.GameObjects.Block;
using game_project.GameObjects.Items;
using game_project.GameObjects.Layout;
using game_project.GameObjects.Link;
using game_project.Levels;

namespace game_project.CollisionResponse
{

    public class DoorCollisionResponse : ECS.Components.CollisionResponse
    {

        Constants.Direction direction = Constants.Direction.DOWN;
        //public bool enabled = true;
        public DoorCollisionResponse(Entity e, Constants.Direction direction) : base(e)
        {
            this.direction = direction;
        }

        public override void Visit(ECS.Components.CollisionResponse other)
        {
            other.ResolveCollision(this);
        }

        public override void ResolveCollision(MovableCollisionResponse other)
        {
            var transform = entity.GetComponent<Transform>();
            transform.position = transform.lastPosition;
        }

        private bool flag = true;

        public override void ResolveCollision(LinkCollisionResponse other)
        {

            Door entity = (Door)this.entity;
            if (entity.doorType == Door.Type.Open)
            {

                if (enabled && flag)
                {
                    //Debug.WriteLine(direction);
                    // be sure we only trigger this collider once
                    enabled = false;
                    // pause game until transition finished
                    // TODO
                    // load new level
                    string currentLevel = LevelManager.currentLevelPath;
                    var split = currentLevel.Split('_');
                    int x = Int16.Parse(split[0]);
                    int y = Int16.Parse(split[1]);

                    switch (direction)
                    {
                        case Constants.Direction.UP:
                            y--;
                            break;
                        case Constants.Direction.LEFT:
                            x--;
                            break;
                        case Constants.Direction.RIGHT:
                            x++;
                            //flag = false;
                            break;
                        case Constants.Direction.DOWN:
                            y++;
                            //flag = false;
                            break;
                    }

                    string nextRoom = x + "_" + y;
                    //Debug.WriteLine(nextRoom);

                    ////LevelManager.activeLevel.Destroy();
                    ////LevelManager.activeLevel = new Level(nextRoom);
                    LevelManager.Load(nextRoom);

                    enabled = false;


                    // disable all door colliders until Link moves into the next room
                    foreach (DoorCollisionResponse d in Door.doorResponses)
                    {
                        d.enabled = false;
                    }

                    // begin motion for Map
                    LevelManager.mapRoot.GetComponent<LevelParentBehavior>().Move(direction);
                    // begin motion for Link
                    LevelManager.link.GetComponent<LinkLevelBehavior>().Move(direction);

                }
            }
            else if (entity.doorType == Door.Type.BombWall) // dont let link through unless opened
            {
                var transform = other.entity.GetComponent<Transform>();
                transform.position = transform.lastPosition;
            } else if (entity.doorType == Door.Type.Locked)
            {
                List<DungeonItems> inven = other.entity.GetComponent<LinkInventory>().dungeonInventory;
                foreach (DungeonItems item in inven)
                {
                    if (item.type == DungeonItems.Types.KEY)
                    {
                        if (item.amount.current > 0)
                        {
                            item.amount.current -= 1;
                            entity.doorType = Door.Type.Open;
                        }
                        break;
                    }
                }
                other.entity.GetComponent<Transform>().position = other.entity.GetComponent<Transform>().lastPosition;
            }
        }


    }
}
