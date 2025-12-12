using UnityEngine;

public class NPCFloatingDialogue : MonoBehaviour
{
    public GameObject dialogueUI; // El canvas flotante
    public float showDistance = 3f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Ocultar al inicio
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        dialogueUI.transform.LookAt(Camera.main.transform);
        dialogueUI.transform.Rotate(0, 180, 0);

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= showDistance)
        {
            dialogueUI.SetActive(true);
        }
        else
        {
            dialogueUI.SetActive(false);
        }
    }
}
