import React, { Component } from "react";
import styled from "styled-components";
import FarmSuggestionItem from "./FarmSuggestionItem";
import { VerticalList } from "../../atoms/List";
import { QuaternaryHeading } from "../../atoms/Heading";

const Root = styled.div`
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  border-radius: ${p => p.theme.borderRadius.normal};
  background-color: ${p => p.theme.color.white};
`;

const List = styled(VerticalList)`
  li {
    margin-bottom: ${p => p.theme.size.exSmall};
  }
`;

export default class FarmSuggestions extends Component {
  constructor(props) {
    super(props);

    let farms = [];
    for (let i = 0; i < 3; i++) {
      farms.push({
        info: "123 Lò Sơn, ấp Gì Đó, xã Không Biết, huyện Cần Đước, Long An",
        name: "Trang trại ông Chín",
        description:
          "Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.",
        url: "/farms/1",

        id: "1212234r5423",
        photoUrl: `${process.env.PUBLIC_URL}/photos/fs.jpg`
      });
    }

    this.state = {
      farms: farms
    };
  }

  render() {
    const { farms } = this.state;
    return (
      <div>
        <QuaternaryHeading>Thăm Quan Nông Trại Khác?</QuaternaryHeading>
        <Root>
          <List>
            {farms &&
              farms.map((farm, index) => (
                <FarmSuggestionItem key={index} farm={farm} index={index} />
              ))}
          </List>
        </Root>
      </div>
    );
  }
}