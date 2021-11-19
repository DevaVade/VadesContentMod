using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using System.Collections.Generic;
using Terraria;

namespace VadesContentMod.Helpers
{
    public class PrimitivePacket
    {
        public readonly List<VertexPositionColorTexture> Vertices = new List<VertexPositionColorTexture>();
        public readonly PrimitiveType Type = PrimitiveType.TriangleList;
        public readonly Texture Texture;
        public readonly Effect Effect;
        public readonly string Pass;

        public PrimitivePacket(Texture2D texture, string pass, Effect specialEffect = null)
        {
            Texture = texture;
            Pass = pass;
            Effect = specialEffect ?? VadesContentMod.TrailEffect;
        }

        public void Add(Vector2 position, Color color, Vector2 TexCoord)
        {
            Vector2 pos = position - Main.screenPosition;
            Vector3 pos2 = new Vector3(pos.X, pos.Y, 0f);
            Vertices.Add(new VertexPositionColorTexture(pos2, color, TexCoord));
        }

        public void Add(Vector3 position, Color color, Vector2 TexCoord)
        {
            Vector2 pos = new Vector2(position.X, position.Y) - Main.screenPosition;
            Vector3 truePos = new Vector3(pos, position.Z);
            Vertices.Add(new VertexPositionColorTexture(truePos, color, TexCoord));
        }

        public void AddStrip(Vector2 pos1, Vector2 pos2, float size1, float size2, float progress1, float progress2, Color color1, Color color2)
        {
            Vector2 dir = (pos2 - pos1).SafeNormalize(Vector2.Zero).RotatedBy(Math.PI / 2);
            Vector2 offset1 = dir * size1;
            Vector2 offset2 = dir * size2;
            Add(pos1 + offset1, color1, new Vector2(progress1, 1));
            Add(pos1 - offset1, color1, new Vector2(progress1, 0));
            Add(pos2 + offset2, color2, new Vector2(progress2, 1));

            Add(pos2 - offset2, color2, new Vector2(progress2, 0));
            Add(pos2 + offset2, color2, new Vector2(progress2, 1));
            Add(pos1 - offset1, color1, new Vector2(progress1, 0));
        }

        public void Send(SpriteBatch spriteBatch)
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;

            if (Vertices.Count >= 3)
            {
                // Save current spriteBatch state
                SpriteSortMode sortMode = SpriteSortMode.Deferred;
                SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
                DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
                RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
                Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
                Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);

                spriteBatch.End();

                device.Textures[0] = Texture;
                device.SamplerStates[0] = SamplerState.LinearWrap;

                device.RasterizerState = RasterizerState.CullNone;

                VertexPositionColorTexture[] vertices = Vertices.ToArray();

                Effect.Parameters["WVP"].SetValue(PrimitiveHelper.GetMatrix());
                Effect.CurrentTechnique.Passes[Pass].Apply();

                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);

                // Load previous spriteBatch state
                spriteBatch.Begin(sortMode, default, samplerState, depthStencilState, rasterizerState, effect, matrix);
            }
        }
    }
}