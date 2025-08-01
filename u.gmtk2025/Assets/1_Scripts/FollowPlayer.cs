using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    private void Awake()
    {
        this.transform.position = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
    }

    private void Update()
    {
        if(player.transform.position != this.transform.position)
        {
            if(Mathf.Abs(this.transform.position.x - player.transform.position.x) > 0.5f)
            {
                float distance = this.transform.position.x - player.transform.position.x;
                this.transform.position = new Vector3(this.transform.position.x - (0.2f *distance), this.transform.position.y, this.transform.position.z);
            }
            if (Mathf.Abs(this.transform.position.z - player.transform.position.z) > 0.5f)
            {
                float distance = this.transform.position.z - player.transform.position.z;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - (0.2f * distance));
            }
        }
    }
}
