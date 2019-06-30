using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
[RequireComponent(typeof(Item))]
[RequireComponent(typeof(Animator))]
public class Cellphone : MonoBehaviour
{
    Flowchart flowchart;
    Animator animator;
    Item cellphoneItem;
    int messageIntensity = 0;

    [Header("Message sending")]
    public bool sendMessagesAutomatically = true;

    [Range(2f, 24f)]
    public float minimumTimeBetweenTexts = 4f;

    float lastMessageReadTime = 0f;
    bool canTriggerNewMessage = true;

    [Tooltip("Set text message interval in seconds. If 0: texts can only be triggered from outside.")]
    public List<float> messageIntervals = new List<float>();

    private void Start()
    {
        flowchart = GetComponent<Flowchart>();
        cellphoneItem = GetComponent<Item>();
        animator = GetComponent<Animator>();

        lastMessageReadTime = Time.time;
    }

    private void Update() => SendNewIntervalMessage();

    public void AddMessageIntensity() => messageIntensity++;

    /*
     * Control when to send an automatic text to the player
     */
    bool SendNewIntervalMessage()
    {
        if (Input.GetKeyDown(KeyCode.G))
            TriggerNewMessage();

        // Exit if player is still in a message
        if (!canTriggerNewMessage)
            return false;

        // Don't send texts if the phone hasn't been picked up yet
        if (!cellphoneItem.isInInventory)
            return false;

        // Return if list cannot provide interval
        if (messageIntensity >= messageIntervals.Count)
            return false;

        float nextMessageInterval = messageIntervals[messageIntensity];

        // Only enable manual triggers or prevent spam
        if (nextMessageInterval <= minimumTimeBetweenTexts)
            return false;

        // Return if too early
        if (Time.time < lastMessageReadTime + nextMessageInterval)
        {
            float timeUntilNextMessage = Time.time - (lastMessageReadTime + nextMessageInterval);
            timeUntilNextMessage = Mathf.Abs(timeUntilNextMessage);

            return false;
        }

        TriggerNewMessage();
        return true;
    }

    /*
     * If possible, trigger a new cell phone message
     */
    public void TriggerNewMessage()
    {
        flowchart.SetIntegerVariable("messageIntensity", messageIntensity);

        // Queue message and take care of animator in dialogue manager
        GameManager.GLOBAL.dialogue.QueueForRead(flowchart, "NewMessage");
    }

    /*
     * Enable phone screen and dismiss messages
     * Disable phone screen and allow messages
     */
    public void EnablePhoneScreen(bool screenOn = true)
    {
        animator.SetBool("hasScreenOn", screenOn);
        canTriggerNewMessage = !screenOn;

        if (!screenOn)
            lastMessageReadTime = Time.time;

        // canTriggerNewMessage = screenOn ? true : canTriggerNewMessage;
    }
}
