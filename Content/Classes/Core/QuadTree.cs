using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core;

internal class QuadTree
{
    private int maxObjects;
    private int maxLevels;

    private int level;

    private Rectangle treeBounds;
    private List<Collider> colliders;
    private QuadTree[] childNodes;

    public QuadTree(int MaxObjects, int MaxLevels, int Level, Rectangle bounds)
    {
        maxObjects = MaxObjects;
        maxLevels = MaxLevels;
        level = Level;
        treeBounds = bounds;
        colliders = new List<Collider>();

        childNodes = new QuadTree[4];
    }

    

    //Clears the QuadTree
    public void Clear()
    {
        colliders.Clear();

        for (int i = 0; i < childNodes.Length; i++)
        {
            if (childNodes[i] != null) //Goes through the tree
            {
                childNodes[i].Clear();
                childNodes[i] = null;
            }
        }
    }

    //Splits the current node into four smaller nodes
    private void Split()
    {
        int childWidth = treeBounds.Width / 2;
        int childHeight = treeBounds.Height / 2;

        childNodes[0] = new QuadTree(maxObjects, maxLevels, level + 1, new Rectangle(treeBounds.X, treeBounds.Y, childWidth, childHeight));
        childNodes[1] = new QuadTree(maxObjects, maxLevels, level + 1, new Rectangle(treeBounds.X + childWidth, treeBounds.Y, childWidth, childHeight));
        childNodes[2] = new QuadTree(maxObjects, maxLevels, level + 1, new Rectangle(treeBounds.X, treeBounds.Y + childHeight, childWidth, childHeight));
        childNodes[3] = new QuadTree(maxObjects, maxLevels, level + 1, new Rectangle(treeBounds.X + childWidth, treeBounds.Y + childHeight, childWidth, childHeight));
    }

    //Which of the four child nodes will a collider belong to?
    private int GetIndex(BoxCollider collider)
    {
        System.Drawing.RectangleF colliderBounds = collider.CollisionBox;

        int index = -1;

        int verticalMidpoint = (int)(treeBounds.Y + treeBounds.Height / 2);
        int horizontalMidpoint = (int)(treeBounds.X + treeBounds.Width / 2);

        index = (colliderBounds.Y < verticalMidpoint && colliderBounds.Y + colliderBounds.Height < verticalMidpoint) ? 0 : index;
        index = (colliderBounds.Y > verticalMidpoint && colliderBounds.Y + colliderBounds.Height > verticalMidpoint) ? 2 : index;

        index = (colliderBounds.X < horizontalMidpoint && colliderBounds.X + colliderBounds.Width < horizontalMidpoint) ? index : index + 1;

        return index;
    }

    //Place a collider into the quadTree
    public void Insert(Collider collider)
    {
        BoxCollider colliderBox = Collider.EnsureBox(collider);

        if (childNodes[0] != null)
        {
            int index = GetIndex(colliderBox);

            if (index != -1)
            {
                childNodes[index].Insert(collider);
                return;
            }
        }

        colliders.Add(collider);

        if (colliders.Count < maxObjects || level >= maxLevels) return;

        if (childNodes[0] == null) Split();

        for (int i = 0; i < colliders.Count; i++)
        {
            BoxCollider colliderToCull = Collider.EnsureBox(colliders[i]);
            int index = GetIndex(colliderToCull);

            if (index != -1)
            {
                childNodes[index].Insert(colliders[i]);
                colliders.Remove(colliders[i]);
                i--;
            }
        }
    }

    public void Remove(Collider collider)
    {
        BoxCollider colliderBox = Collider.EnsureBox(collider);

        if (childNodes[0] != null)
        {
            int index = GetIndex(colliderBox);

            if (index != -1)
            {
                childNodes[index].Remove(collider);
                return;
            }
        }

        colliders.Remove(collider);
    }

    public List<Collider> Retrieve(List<Collider> returnColliders, Collider collider)
    {
        BoxCollider boxCollider = Collider.EnsureBox(collider);

        int index = GetIndex(boxCollider);
        if (index != -1 && childNodes[0] != null) return childNodes[index].Retrieve(returnColliders, collider);

        if (returnColliders == null) returnColliders = new List<Collider>();
        returnColliders.AddRange(colliders);

        return returnColliders;
    }
}
