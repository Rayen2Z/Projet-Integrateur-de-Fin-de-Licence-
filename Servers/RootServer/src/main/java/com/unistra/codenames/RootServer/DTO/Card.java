package com.unistra.codenames.RootServer.DTO;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

import java.util.HashMap;
import java.util.Map;

@Getter
@Setter
@AllArgsConstructor
public class Card {
    @Getter
    @AllArgsConstructor
    public enum Property {
        BLUE(0),
        RED(1),
        ANONYMOUS(2),
        BLACK(3);

        private Integer value;
        private static Map map = new HashMap<>();
        static {
            for (Card.Property item : Card.Property.values()) {
                map.put(item.getValue(), item);
            }
        }
        public static Card.Property parseFromInteger(int integer) {
            return (Card.Property) map.get(integer);
        }
    }

    String word;
    Property property;
    Boolean turnedUp;

}
