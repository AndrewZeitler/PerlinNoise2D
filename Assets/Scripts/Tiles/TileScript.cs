using UnityEngine;

namespace Tiles {

    public class TileScript : MonoBehaviour {
        GameObject outline;

        public void CreateOutline(SpriteRenderer sprite){
            if(outline != null || sprite.sprite == null) return;
            outline = new GameObject();
            outline = Instantiate(outline, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.05f), Quaternion.identity);
            outline.transform.SetParent(this.transform);
            SpriteRenderer spriteRenderer = outline.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite.sprite;
            spriteRenderer.color = new Color(0.1f, 0.3f, 0.1f, 0.2f);
            spriteRenderer.sortingLayerID = sprite.sortingLayerID;
            spriteRenderer.sortingOrder = sprite.sortingOrder;
        }

        public void DeleteOutline(){
            Destroy(outline);
            outline = null;
        }

    }

}