import React, { Component } from "react";
import styled from "styled-components";
import { HorizontalList } from "../../atoms/List";
import DropdownButton from "../../molecules/DropdownButton";
import ProfileNavLink from "../../molecules/Links/ProfileNavLink";

const Root = styled.div`
  position: relative;
`;

const ListItem = styled.li`
  display: inline-block;
  margin: 0 ${(p) => p.theme.size.distance};

  a.actived {
    color: ${(p) => p.theme.color.primaryDark};
    text-decoration: none;
    font-weight: 600;
    border-bottom: 3px solid ${(p) => p.theme.color.primary};
  }

  :hover a {
    color: ${(p) => p.theme.color.primaryDark};
    text-decoration: none;
  }

  a {
    color: ${(p) => p.theme.color.neutral};
    font-weight: 500;
    font-size: ${(p) => p.theme.fontSize.small};
    border: 0;
    height: ${(p) => p.theme.size.medium};
    line-height: ${(p) => p.theme.size.medium};
    display: inline-block;
    height: 100%;
  }
`;

const UserDropdown = styled(DropdownButton)`
  padding: 7px;
`;

export default (class extends Component {
  render() {
    const { className, userId, baseUrl } = this.props;
    return (
      <Root>
        <div className="row">
          <div className="col-auto mr-auto">
            <HorizontalList className={className}>
              <ListItem>
                <ProfileNavLink
                  {...this.props}
                  userId={userId}
                  baseUrl={baseUrl}
                >
                  Feed
                </ProfileNavLink>
              </ListItem>
              <ListItem>
                <ProfileNavLink
                  pageNav="posts"
                  {...this.props}
                  userId={userId}
                  baseUrl={baseUrl}
                >
                  Articles
                </ProfileNavLink>
              </ListItem>
              <ListItem>
                <ProfileNavLink
                  pageNav="products"
                  {...this.props}
                  userId={userId}
                >
                  Products
                </ProfileNavLink>
              </ListItem>
              <ListItem>
                <ProfileNavLink
                  pageNav="farms"
                  {...this.props}
                  userId={userId}
                  baseUrl={baseUrl}
                >
                  Farms
                </ProfileNavLink>
              </ListItem>
              <ListItem>
                <ProfileNavLink
                  pageNav="followings"
                  {...this.props}
                  userId={userId}
                >
                  Following
                </ProfileNavLink>
              </ListItem>
              <ListItem>
                <ProfileNavLink
                  pageNav="about"
                  {...this.props}
                  userId={userId}
                  baseUrl={baseUrl}
                >
                  About Me
                </ProfileNavLink>
              </ListItem>
            </HorizontalList>
          </div>
          <div className="col-auto">
            <UserDropdown
              icon="ellipsis-v"
              dropdown={[
                {
                  url: `${baseUrl}/${userId}/update`,
                  name: "Update My Information",
                  isNav: true,
                },
                {
                  url: `${baseUrl}/${userId}/security`,
                  name: "Security",
                  isNav: true,
                },
              ]}
            />
          </div>
        </div>
      </Root>
    );
  }
});
