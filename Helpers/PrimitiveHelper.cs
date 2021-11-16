using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace VadesContentMod.Helpers
{
    public class MatrixCollection
    {
        public Matrix View;
        public Matrix Projection;
    }

    public static class PrimitiveHelper
    {
        public static MatrixCollection GetMatrixes()
        {
            MatrixCollection matrixes = new MatrixCollection();
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            int width = device.Viewport.Width;
            int height = device.Viewport.Height;
            Vector2 zoom = Main.GameViewMatrix.Zoom;
            matrixes.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
            matrixes.Projection = Matrix.CreateOrthographic(width, height, 0, 1000);
            return matrixes;
        }

        public static Matrix GetMatrix()
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            int width = device.Viewport.Width;
            int height = device.Viewport.Height;
            Vector2 zoom = Main.GameViewMatrix.Zoom;
            Matrix View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
            Matrix Projection = Matrix.CreateOrthographic(width, height, 0, 1000);
            return View * Projection;
        }

        public static Vector2 GetRotation(Vector2[] oldPos, int index)
        {
            if (oldPos.Length == 1)
                return oldPos[0];

            if (index == 0)
            {
                return Vector2.Normalize(oldPos[1] - oldPos[0]).RotatedBy(MathHelper.PiOver2);
            }

            return Vector2.Normalize(oldPos[index + (index == oldPos.Length - 1 ? 0 : 1)] - oldPos[index - 1]).RotatedBy(MathHelper.PiOver2);
        }

        public static void SetBasicEffectParameters(this Effect effect)
        {
            effect.Parameters["WVP"].SetValue(GetMatrix());
        }
    }
}