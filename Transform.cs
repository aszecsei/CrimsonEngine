using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine
{
    public class Transform : Component
    {
        private Transform _parent;
        public Transform Parent
        {
            get
            {
                return _parent;
            }

            set
            {
                if (_parent != null)
                    _parent._children.Remove(this);

                _parent = value;
                _parent._children.Add(this);
            }
        }

public Vector3 LocalPosition { get; set; }
public Vector3 GlobalPosition
{
    get
    {
        if (Parent != null)
        {
            // rotate and scale the local position
            Matrix transformationMatrix = Matrix.CreateScale(new Vector3(GlobalScale.X, GlobalScale.Y, 1)) * Matrix.CreateRotationZ(GlobalRotation);
            Vector3 transformedLocal = Vector3.Transform(LocalPosition, transformationMatrix);
            return transformedLocal + Parent.GlobalPosition;
        }
                    
        return LocalPosition;
    }

    set
    {
        if (Parent != null)
        {
            // rotate and scale the local position
            Matrix transformationMatrix = Matrix.CreateScale(new Vector3(GlobalScale.X, GlobalScale.Y, 1)) * Matrix.CreateRotationZ(GlobalRotation);
            Vector3 transformedGlobal = Vector3.Transform(value, Matrix.Invert(transformationMatrix));
            LocalPosition = transformedGlobal;
            return;
        }

        LocalPosition = value;
    }
}

        /// <summary>
        /// The rotation of the transform, in local space (in radians).
        /// </summary>
public float LocalRotation { get; set; }
        /// <summary>
        /// The rotation of the transform, in world space (in radians).
        /// </summary>
public float GlobalRotation
{
    get
    {
        if(Parent != null)
        {
            return LocalRotation + Parent.GlobalRotation;
        }

        return LocalRotation;
    }

    set
    {
        if (Parent != null)
        {
            LocalRotation = value - Parent.GlobalRotation;
            return;
        }

        LocalRotation = value;
    }
}

public Vector2 LocalScale { get; set; }
public Vector2 GlobalScale
{
    get
    {
        if (Parent != null)
        {
            return LocalScale * Parent.GlobalScale;
        }

        return LocalScale;
    }

    set
    {
        if (Parent != null)
        {
            LocalScale = value / Parent.GlobalScale;
            return;
        }

        LocalScale = value;
    }
}

        private List<Transform> _children;

        public List<Transform> GetChildren()
        {
            return new List<Transform>(_children);
        }

        public Transform()
        {
            _children = new List<Transform>();
            _parent = null;

            LocalPosition = Vector3.Zero;
            LocalRotation = 0;
            LocalScale = Vector2.One;
        }
    }
}
