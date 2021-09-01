# xamarin-android-example
# Grouping a list of checkboxes with a ListView in Xamarin.Android
Working with lists is presented in sufficient detail in various sources. In particular, [this book](https://bhv.ru/product/razrabotka-android-prilozhenij-na-s-s-ispolzovaniem-xamarin-s-nulya/) looked at an example of grouping patient vital sign results (blood pressure and heart rate) by measurement date using the native ListView component. Grouping your data together is easy. But if you add the ability to mark list items inside a subgroup, then the solution becomes not trivial, since the ListView does not provide this feature by default. Let's consider one of the options for implementing such an interface.

Suppose we have a list of countries, for each of which a list of significant cities is given. Let's imagine these cities, grouped by each of the states, with the ability to mark them in an arbitrary way and perform simple processing of the selected substrings, for example, sending the number of marked cities by clicking a button inside the group. To do this, we need to do the following in the created project based on an empty template:

* Define a ListView in the starting activity;
* Define markup for displaying one item in the list;
* Define the data model for the view;
* Define a custom version of the adapter for the ListView that will use the generated data model.

A key feature of this solution will be to dynamically create Checkbox widgets on each list entry inside a LinearLayout container.
